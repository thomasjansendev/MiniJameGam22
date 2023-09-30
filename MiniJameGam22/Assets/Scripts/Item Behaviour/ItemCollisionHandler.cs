using UnityEngine;

namespace Item_Behaviour
{
    public class ItemCollisionHandler : MonoBehaviour
    {
    
        private bool addedToBasket; 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            if (!addedToBasket)
            {
                other.gameObject.GetComponent<CartContentManager>().AddItemToCart();
                addedToBasket = true;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                gameObject.tag = "InCart";
            }
        }
    
    }
}
