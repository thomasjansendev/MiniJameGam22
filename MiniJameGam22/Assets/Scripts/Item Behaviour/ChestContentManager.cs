using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContentManager : MonoBehaviour
{

    private int _itemQuantityInCart;
    
    public int ItemQuantityInChest { get; private set; }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        print(other.gameObject.name);
        var cartContentManager = other.gameObject.GetComponent<CartContentManager>();
        _itemQuantityInCart = cartContentManager.ItemCountInCart;
        if (_itemQuantityInCart != 0)
        {
            GetComponent<AudioSource>().Play();
        }
        ItemQuantityInChest += _itemQuantityInCart;
        cartContentManager.EmptyCart();
        print("item quantity in chest: " + ItemQuantityInChest);
        foreach (var obj in GameObject.FindGameObjectsWithTag("InCart"))
        {
            Destroy(obj.transform.parent.gameObject);
        }
    }
}
