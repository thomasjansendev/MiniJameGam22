using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public enum PathfindingTarget
{
    Player,
    Waypointing,
    Undefined
}


public class EnemyPathfinding : MonoBehaviour
{
    public Transform transformTarget;
    private AudioSource audioSource;
    private NavMeshAgent agent;
    private GameObject player;
    private List<GameObject> waypoints = new();

    [SerializeField] private GameObject emptyGameObject;
    [SerializeField] private int minimumPathLength;
    private int curWaypoint;
    [SerializeField] private float waypointSwitchMag;
    [SerializeField] private float enemyLostSightMag;
    public PathfindingTarget target = PathfindingTarget.Undefined;
    private bool delaySwitching;
    private WorldPoints points;
    public bool playedAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        points = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldPoints>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        GenerateWaypoints();
    }


    private void GenerateWaypoints()
    {
        // trial and error to make sure no collision on waypoint spawn and path length long enough
        for (int i = 0; i < 100; i++)
        {
            List<Vector2> path = points.GenerateRandomPath();

            if ((path[0] - path[1]).magnitude < minimumPathLength)
            {
                continue;
            }

            if (points.CollisionAtPoint(path[0]) || points.CollisionAtPoint(path[1]))
            {
                continue;
            }

            waypoints.Add(Instantiate(emptyGameObject, path[1], Quaternion.identity));
            waypoints.Add(Instantiate(emptyGameObject, path[0], Quaternion.identity));
            return;
        }

        throw new("Not able to generate items");
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
            playedAudio = false;
            print("reset status");
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
                if (!playedAudio)
                {
                    audioSource.pitch = Rand.Between(0.5f, 1.5f);
                    audioSource.Play();
                    playedAudio = true;
                }
            }
        }

        //Method to draw the ray in scene for debug purpose
        Debug.DrawRay(transform.position, dir * 20, Color.red);
    }
}