using UnityEngine;

namespace Item_Behaviour
{
    public class ItemCollisionHandler : MonoBehaviour
    {
        public bool alreadyAddedToBasket;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            if (!alreadyAddedToBasket)
            {
                other.gameObject.GetComponent<CartContentManager>().AddItemToCart();
                alreadyAddedToBasket = true;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                gameObject.tag = "InCart";
            }
        }
    }
}