using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float moveForce;
    private Vector2 _moveForwardVector;
    private float _moveInput; //to store forward/back input

    [SerializeField] private float turnRate;
    [SerializeField] private float goBackSpeed;
    private float _turnInput;
    private Vector3 playerStartPos;
    [NonSerialized] public bool movingToStartPos;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerStartPos = GameObject.FindGameObjectWithTag("PlayerStartPos").transform.position;
        transform.position = playerStartPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate();
        Turn();
        OptionalMoveBackToStart();
    }

    private void OptionalMoveBackToStart()
    {
        if (!movingToStartPos)
        {
            return;
        }
        
        var step = goBackSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, playerStartPos, step);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, Vector3.up), 0.04f);
        if ((transform.position - playerStartPos).magnitude < 0.01f)
        {
            movingToStartPos = false;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }


    private void Accelerate()
    {
        if (movingToStartPos)
        {
            return;
        }

        _moveForwardVector = transform.up * moveForce;
        _rb.AddForce(_moveInput * _moveForwardVector);
    }

    private void Turn()
    {
        if (movingToStartPos)
        {
            return;
        }

        float turnRateValue = turnRate * _turnInput; //if no input then turnRateValue = 0
        _rb.MoveRotation(_rb.rotation + turnRateValue);
    }

    public void MovePlayerBackToStart()
    {
        print("starting move back");
        movingToStartPos = true;
        GetComponent<BoxCollider2D>().enabled = false;
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