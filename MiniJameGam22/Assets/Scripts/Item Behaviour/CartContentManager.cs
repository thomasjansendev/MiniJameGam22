using UnityEngine;

public class CartContentManager : MonoBehaviour
{
    [SerializeField] private float massModifier;
    private float _initMass;
    private Rigidbody2D _rb;

    public int ItemCountInCart { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _initMass = _rb.mass;
        ItemCountInCart = 0;
    }

    public void AddItemToCart()
    {
        ItemCountInCart++;
        _rb.mass += massModifier;
        print("# items in cart: " + ItemCountInCart);
        print("mass of cart: " + _rb.mass);
    }
    
    public void EmptyCart()
    {
        ItemCountInCart = 0;
        _rb.mass = _initMass;
        print("# items in cart: " + ItemCountInCart);
        print("mass of cart: " + _rb.mass);
    }
    
}