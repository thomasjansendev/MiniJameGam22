using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CartContentManager : MonoBehaviour
{
    [SerializeField] private ItemPickUp itemClass;
    
    private int _itemQuantity;
    private float _playerWeight;
    private Rigidbody2D _rb;

    private Collider2D _playerCollider;
    [SerializeField] private float massModifier;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        itemClass.OnItemAddedToCart += OnItemAddedToCartHandler;
    }

    private void Update()
    {

    }

    private void OnItemAddedToCartHandler()
    {
        _itemQuantity++;
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
