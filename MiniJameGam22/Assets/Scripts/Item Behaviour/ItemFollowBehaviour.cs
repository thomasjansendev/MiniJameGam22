using UnityEngine;
using Utilities;

public class ItemFollowBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scatterForce;
    private Rigidbody2D _itemRigidbody;
    private Transform _playerTransform;
    private bool _playerDetected;
    private CapsuleCollider2D _collider;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _itemRigidbody = GetComponent<Rigidbody2D>();
        _playerDetected = false;
        _collider = GetComponentInChildren<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
        CheckIfStuckInWall();
    }

    private void CheckIfStuckInWall()
    {
        if (_playerDetected && (transform.position - _playerTransform.position).magnitude > 2f)
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
        {
            return;
        }

        _playerTransform = other.transform;
        _playerDetected = true;
    }

    private void MoveTowardsPlayer()
    {
        if (!_playerDetected || _gameController.gameState != GameState.Play)
            return;

        Vector2 moveDir = _playerTransform.position - transform.position;
        _itemRigidbody.AddForce(moveDir * moveSpeed);
    }

    public void Scatter()
    {
        _playerDetected = false;
        var dir = (Vector2)(Quaternion.Euler(0, 0, Rand.Between(0, 360)) * Vector2.up);
        _itemRigidbody.AddForce(dir * scatterForce);
        transform.GetChild(0).tag = "Untagged";
    }
}