using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private EnemyPathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = GetComponent<EnemyPathfinding>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("caught");
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().MovePlayerBackToStart();
            pathfinding.target = PathfindingTarget.Waypointing;
            // TODO lose items? Maybe they could fly off?
            // TODO maybe freeze enemies while you go back to start pos to look cool?
        }

    }
}