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
        return rnd.Next((int)(bottomLeftCorner.x * rndScaleFactor), (int)(topRightCorner.x * rndScaleFactor)) /
               rndScaleFactor;
    }

    private float RandomY()
    {
        return rnd.Next((int)(bottomLeftCorner.y * rndScaleFactor), (int)(topRightCorner.y * rndScaleFactor)) /
               rndScaleFactor;
    }

    private void GenerateWaypoints()
    {
        bool horizontal = rnd.Next(1) == 0;
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

        waypoints.Add(Instantiate(emptyGameObject, waypoint1, Quaternion.identity));
        waypoints.Add(Instantiate(emptyGameObject, waypoint2, Quaternion.identity));
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
        print("trying to lose sight");
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