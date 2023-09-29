using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector2 _moveDir; //to get player input vector
    private Vector2 _moveVel; //to get resultant velocity vector

    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.AddForce(_moveVel); 
    }
    
    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>(); 
        _moveVel = _moveDir.normalized * moveSpeed; 
        Debug.Log(_moveVel);
    }
    
}
