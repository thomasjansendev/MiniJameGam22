using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class ItemFollowBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scatterForce;
    private Rigidbody2D _itemRigidbody;
    private Transform _playerTransform;
    [FormerlySerializedAs("_playerDetected")] public bool _itemDetectedPlayer;
    private CapsuleCollider2D _collider;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _itemRigidbody = GetComponent<Rigidbody2D>();
        _itemDetectedPlayer = false;
        _collider = GetComponentInChildren<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
        CheckIfStuckInWall();
        if (_itemDetectedPlayer)
        {
            transform.GetChild(0).tag = "InCart";
        }
        else
        {
            transform.GetChild(0).tag = "Untagged";
        }
    }

    private void CheckIfStuckInWall()
    {
        if (_itemDetectedPlayer && (transform.position - _playerTransform.position).magnitude > 2f)
        {
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
            Delay.Method(() => ResetCollider(), 1f);
        }
    }

    private void ResetCollider()
    {
        if (_collider != null)
        {
            _collider.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        
        if (other.gameObject.GetComponent<PlayerController>().movingToStartPos)
            return;

        _playerTransform = other.transform; //to use in the MoveTowardsPlayer() method
        _itemDetectedPlayer = true;
    }

    private void MoveTowardsPlayer()
    {
        if (!_itemDetectedPlayer || _gameController.gameState != GameState.Play)
            return;

        Vector2 moveDir = _playerTransform.position - transform.position;
        _itemRigidbody.AddForce(moveDir * moveSpeed);
    }

    public void Scatter()
    {
        _itemDetectedPlayer = false;
        var dir = (Vector2)(Quaternion.Euler(0, 0, Rand.Between(0, 360)) * Vector2.up);
        _itemRigidbody.AddForce(dir * scatterForce);
        GetComponent<ItemSounds>().PlayScatter();
    }
}