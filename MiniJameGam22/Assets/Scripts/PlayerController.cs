using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;


    [SerializeField] private float moveForce;
    private Vector2 _moveForwardVector;
    private float _moveInput; //to store forward/back input

    [SerializeField] private float turnRate;
    private float _turnInput;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate();
        Turn();
    }

    private void Accelerate()
    {
        _moveForwardVector = transform.up * moveForce;
        _rb.AddForce(_moveInput * _moveForwardVector);
    }

    private void Turn()
    {
        float turnRateValue = turnRate * _turnInput; //if no input then turnRate = 0
        _rb.MoveRotation(_rb.rotation + turnRateValue);
    }

    #region Input Handling

    private void OnAccelerate(InputValue value)
    {
        _moveInput = value.Get<float>();
    }

    private void OnTurn(InputValue value)
    {
        _turnInput = value.Get<float>();
    }

    void OnQuit()
    {
        // def allows this to work in the editor or in regular game
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion
}