using Item_Behaviour;
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
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().MovePlayerBackToStart();
            other.gameObject.GetComponentInChildren<CartContentManager>().EmptyCart();
            pathfinding.target = PathfindingTarget.Waypointing;
            foreach (var obj in GameObject.FindGameObjectsWithTag("InCart"))
            {
                obj.GetComponentInParent<ItemFollowBehaviour>().Scatter();
                obj.GetComponent<ItemCollisionHandler>().alreadyAddedToBasket = false;
            } 
            // TODO maybe freeze enemies while you go back to start pos to look cool?
        }

    }
}