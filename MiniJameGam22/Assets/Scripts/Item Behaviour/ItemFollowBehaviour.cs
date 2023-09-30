using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

public class ItemFollowBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scatterForce;
    private Rigidbody2D _itemRigidbody;
    private Transform _playerTransform;
    private bool _playerDetected;
    private CapsuleCollider2D collider;


    private void Start()
    {
        _itemRigidbody = GetComponent<Rigidbody2D>();
        _playerDetected = false;
        collider = GetComponentInChildren<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
        CheckIfStuckInWall();
    }

    private void CheckIfStuckInWall()
    {
        if (_playerDetected && (transform.position - _playerTransform.position).magnitude > 2f)
        {
            if (collider != null)
            {
                collider.isTrigger = true;
            }
            Delay.Method(() => ResetCollider(), 1f);
        }
    }

    private void ResetCollider()
    {
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        if (other.gameObject.GetComponent<PlayerController>().movingToStartPos)
        {
            print("moving so don't follow!");
            return;
        }

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

    public void Scatter()
    {
        _playerDetected = false;
        var dir = (Vector2)(Quaternion.Euler(0, 0, Rand.Between(0, 360)) * Vector2.up);
        _itemRigidbody.AddForce(dir * scatterForce);
    }
}