using System;
using UnityEngine;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private bool _movementLocked;
    public float moveSpeed;
    [NonSerialized] public Vector2 MovementInput;
    public ContactFilter2D movementFilter;
    [SerializeField] private float collisionOffset = 0.05f;
    private List<RaycastHit2D> _castCollisions = new();

    private bool _isMoving;
    private int _direction; // 0 for down, 1 for sideways, 2 for up


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_movementLocked)
        {
            // If movement input is not 0, try to move
            if (MovementInput != Vector2.zero)
            {
                bool success = TryMove(MovementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(MovementInput.x, 0));
                }

                if (!success)
                {
                    TryMove(new Vector2(0, MovementInput.y));
                }

                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }
        
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = _rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                _castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime +
                collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                _rb.MovePosition(_rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
                return true;
            }
        }

        // Can't move if there's no direction to move in
        return false;
    }
}