using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldPoints : MonoBehaviour
{
    private System.Random rnd;
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private Vector2 topRightCorner;
    [SerializeField] private int rndGranularity;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rnd = new System.Random();
    }

    private float RandomX()
    {
        return (float)rnd.Next((int)(bottomLeftCorner.x * rndGranularity), (int)(topRightCorner.x * rndGranularity)) /
               rndGranularity;
    }

    private float RandomY()
    {
        return (float)rnd.Next((int)(bottomLeftCorner.y * rndGranularity), (int)(topRightCorner.y * rndGranularity)) /
               rndGranularity;
    }

    public List<Vector2> GenerateRandomPath()
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

    public bool CollisionAtPoint(Vector2 pos)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(pos, path);
        return path.status != NavMeshPathStatus.PathComplete;
    }
}