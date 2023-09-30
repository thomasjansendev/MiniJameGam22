using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class WorldPoints : MonoBehaviour
{
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private Vector2 topRightCorner;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private float RandomX()
    {
        return Rand.Between(bottomLeftCorner.x, topRightCorner.x);
    }

    private float RandomY()
    {
        return Rand.Between(bottomLeftCorner.y, topRightCorner.y);
    }

    public List<Vector2> GenerateRandomPath()
    {
        List<Vector2> path = new();
        bool horizontal = Rand.CoinFlip();
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