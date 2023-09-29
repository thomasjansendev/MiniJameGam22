using System;
using System.Collections.Generic;
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
    [SerializeField] private int rndScaleFactor;
    private int curWaypoint;
    [SerializeField] private float waypointSwitchMag;
    public PathfindingTarget target = PathfindingTarget.Undefined;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
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

        waypoints.Add(Instantiate(new GameObject(), waypoint1, Quaternion.identity));
        waypoints.Add(Instantiate(new GameObject(), waypoint2, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
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

        agent.SetDestination(transformTarget.position);

        if ((transform.position - transformTarget.position).magnitude < waypointSwitchMag)
        {
            print(waypoints[0]);
            print(waypoints[1]);
            curWaypoint += 1;
            curWaypoint %= 2; // cycle between 0 - 1 - 0 - 1 - ...
        }
    }
}