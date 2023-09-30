using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemFollowBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D _itemRigidbody;
    private Transform _playerTransform;
    private bool _playerDetected;


    // Start is called before the first frame update
    private void Start()
    {
        _itemRigidbody = GetComponent<Rigidbody2D>();
        _playerDetected = false;
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
    }
    
    private void MoveTowardsPlayer()
    {
        if (!_playerDetected)
            return;

        Vector2 moveDir = _playerTransform.position - transform.position;
        _itemRigidbody.AddForce(moveDir * moveSpeed);
    }
    
}
