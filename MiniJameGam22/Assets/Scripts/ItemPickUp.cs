using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float destroyDelay;
    private Collider2D _outerRadius;
    private Rigidbody2D _itemRigidbody;

    private bool _playerDetected;
    private Transform _playerTransform;
    private Collider2D _itemCollider;

    // Start is called before the first frame update
    void Start()
    {
        _itemRigidbody = GetComponent<Rigidbody2D>();
        _outerRadius = GetComponent<CircleCollider2D>();
        _itemCollider = GetComponent<BoxCollider2D>();
        _playerDetected = false;
        ;
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        _playerTransform = other.transform;
        _playerDetected = true;
        Debug.Log(_playerDetected);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        other.gameObject.GetComponent<CartContentManager>().AddItemToCart();
        Delay.DestroyObj(gameObject, destroyDelay);
    }

    private void MoveTowardsPlayer()
    {
        if (!_playerDetected)
            return;

        Vector2 moveDir = _playerTransform.position - transform.position;
        _itemRigidbody.AddForce(moveDir * moveSpeed);
    }
}