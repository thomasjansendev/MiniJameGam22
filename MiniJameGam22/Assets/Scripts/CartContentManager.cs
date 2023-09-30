using UnityEngine;

public class CartContentManager : MonoBehaviour
{
    private int _itemQuantity;
    private float _playerWeight;
    private Rigidbody2D _rb;

    private Collider2D _playerCollider;
    [SerializeField] private float massModifier;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        print("up and running");
    }

    public void AddItemToCart()
    {
        _itemQuantity++;
        print(_itemQuantity);
        _rb.mass += massModifier;
        Debug.Log("additemtocart");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Item"))
            return;

        Debug.Log("itemcollisiondetected");
    }
}