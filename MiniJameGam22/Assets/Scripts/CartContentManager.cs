using UnityEngine;

public class CartContentManager : MonoBehaviour
{
    [SerializeField] private float massModifier;
    private int _itemQuantity;
    private Rigidbody2D _rb;
    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        print("cart is up and running");
    }

    public void AddItemToCart()
    {
        _itemQuantity++;
        _rb.mass += massModifier;
        print("# items in cart: " + _itemQuantity);
        print("mass of cart: " + _rb.mass);
    }
    
}