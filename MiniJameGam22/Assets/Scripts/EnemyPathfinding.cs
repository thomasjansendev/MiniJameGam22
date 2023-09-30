using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum PathfindingTarget
{
    Player,
    Waypointing,
    Undefined
}


public class EnemyPathfinding : MonoBehaviour
{
    public Transform transformTarget;
    private NavMeshAgent agent;
    private GameObject player;
    private List<GameObject> waypoints = new();
    private System.Random rnd;
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private Vector2 topRightCorner;
    [SerializeField] private GameObject emptyGameObject;
    [SerializeField] private int rndScaleFactor;
    [SerializeField] private int minimumPathLength;
    private int curWaypoint;
    [SerializeField] private float waypointSwitchMag;
    [SerializeField] private float enemyLostSightMag;
    public PathfindingTarget target = PathfindingTarget.Undefined;
    private bool delaySwitching;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        rnd = new System.Random();
        GenerateWaypoints();
    }

    private float RandomX()
    {
        return (float)rnd.Next((int)(bottomLeftCorner.x * rndScaleFactor), (int)(topRightCorner.x * rndScaleFactor)) /
               rndScaleFactor;
    }

    private float RandomY()
    {
        return rnd.Next((int)(bottomLeftCorner.y * rndScaleFactor), (int)(topRightCorner.y * rndScaleFactor)) /
               rndScaleFactor;
    }

    private List<Vector2> GenerateRandomPath()
    {
        List<Vector2> path = new();
        bool horizontal = rnd.Next(2) == 0;
        Vector2 waypoint1, waypoint2;

        if (horizontal)
        {
            float y = RandomY();
            waypoint1 = new Vector2(RandomX(), y);
            waypoint2 = new Vector2(RandomX(), y);
        }
        else
        {
            float x = RandomX();
            waypoint1 = new Vector2(x, RandomY());
            waypoint2 = new Vector2(x, RandomY());
        }

        path.Add(waypoint1);
        path.Add(waypoint2);
        return path;
    }

    private bool CollisionAtPoint(Vector2 pos)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(pos, path);
        return path.status != NavMeshPathStatus.PathComplete;
    }

    private void GenerateWaypoints()
    {
        // trial and error to make sure no collision on waypoint spawn and path length long enough
        while (true)
        {
            List<Vector2> path = GenerateRandomPath();
            print(path[0]);
            print(path[1]);

            if ((path[0] - path[1]).magnitude < minimumPathLength)
            {
                print("failed cause too small");
                continue;
            }

            if (CollisionAtPoint(path[0]) || CollisionAtPoint(path[1]))
            {
                print("failed cause collision");
                continue;
            }

            waypoints.Add(Instantiate(emptyGameObject, path[1], Quaternion.identity));
            waypoints.Add(Instantiate(emptyGameObject, path[0], Quaternion.identity));
            return;
        }
    }

    private void FaceTarget()
    {
        if (agent.velocity.magnitude < 0.001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(Vector3.forward, agent.velocity.normalized);
    }


    private Transform ChooseTarget()
    {
        if (target == PathfindingTarget.Player)
        {
            transformTarget = player.transform;
        }
        else if (target == PathfindingTarget.Waypointing)
        {
            transformTarget = waypoints[curWaypoint].transform;
        }
        else
        {
            transformTarget = transform;
        }

        return transformTarget;
    }

    private void TrySwitchWaypoint()
    {
        if ((transform.position - transformTarget.position).magnitude > waypointSwitchMag || delaySwitching)
        {
            return;
        }

        curWaypoint += 1;
        curWaypoint %= 2; // cycle between 0 - 1 - 0 - 1 - ...
        delaySwitching = true;
        Delay.Method(() => delaySwitching = false, 1f); // give cooldown between switching waypoints
    }

    private void TryLoseSightOfPlayer()
    {
        if ((transform.position - transformTarget.position).magnitude > enemyLostSightMag)
        {
            target = PathfindingTarget.Waypointing;
        }
    }

    void Update()
    {
        agent.SetDestination(ChooseTarget().position);
        FaceTarget();
        if (target == PathfindingTarget.Waypointing)
        {
            TrySwitchWaypoint();
        }

        if (target == PathfindingTarget.Player)
        {
            TryLoseSightOfPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // this took soooooo long
        Vector3 dir = (other.transform.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 20);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Obstruction"))
            {
                return;
            }

            if (hit.collider.CompareTag("Player"))
            {
                target = PathfindingTarget.Player;
            }
        }

        //Method to draw the ray in scene for debug purpose
        Debug.DrawRay(transform.position, dir * 20, Color.red);
    }
}