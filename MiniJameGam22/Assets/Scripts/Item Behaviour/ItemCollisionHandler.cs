using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    
    [SerializeField] private float destroyDelay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        other.gameObject.GetComponent<CartContentManager>().AddItemToCart();
        Delay.DestroyObj(transform.root.gameObject, destroyDelay);
    }
    
}
