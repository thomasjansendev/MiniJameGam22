using System;
using UnityEditor;
using UnityEngine;

namespace Item_Behaviour
{
    public class ItemCollisionHandler : MonoBehaviour
    {
        public bool alreadyAddedToBasket;

        private void Update()
        {
            // jank as f**k fix to a problem I don't understand:
            // the collider keeps getting changed to isTrigger = false when colliding with the
            // player
            // and I don't know why
            //GetComponent<CapsuleCollider2D>().isTrigger = true;  
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
           
            if (!alreadyAddedToBasket)
            {
                other.gameObject.GetComponent<CartContentManager>().AddItemToCart();
                alreadyAddedToBasket = true; // does this get reset when objects are scattered ?
                gameObject.tag = "InCart";
                GetComponentInParent<ItemSounds>().PlayPickup();
            }
        }
    }
}