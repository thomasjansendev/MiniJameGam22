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
    private AudioSource audioSource;

    public bool GameStartPressed { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerStartPos = GameObject.FindGameObjectWithTag("PlayerStartPos").transform.position;
        transform.position = playerStartPos;
        GameStartPressed = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate();
        Turn();
        OptionalMoveBackToStart();
        PlayTrolleyAudio();
    }

    public void FreezePlayer()
    {
        _rb.velocity = new Vector3(0  ,0, 0);
        _rb.angularVelocity = 0;
    }

    private void PlayTrolleyAudio()
    {
        if (_rb.velocity.magnitude > 2f)
        {
            audioSource.volume = 0.05f;
        }
        else
        {
            audioSource.volume /= 1 + (10 / _rb.velocity.magnitude) ;  // scales down volume quicker if velocity low
        }
    }


    private void Accelerate()
    {
        if (movingToStartPos)
            return;

        _moveForwardVector = transform.up * moveForce;
        _rb.AddForce(_moveInput * _moveForwardVector);
    }

    private void Turn()
    {
        if (movingToStartPos)
            return;

        float turnRateValue = turnRate * _turnInput; //if no input then turnRateValue = 0
        _rb.MoveRotation(_rb.rotation + turnRateValue);
    }

    private void OptionalMoveBackToStart()
    {
        if (!movingToStartPos)
            return;

        var step = goBackSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, playerStartPos, step);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, Vector3.up), 0.04f);
        if ((transform.position - playerStartPos).magnitude < 0.01f)
        {
            movingToStartPos = false;
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
    }

    public void MovePlayerBackToStart()
    {
        print("starting move back");
        movingToStartPos = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    #region Input Handling

    private void OnAccelerate(InputValue value)
    {
        if (!GameStartPressed) // to prevent player from moving before exiting start screen
            return;

        _moveInput = value.Get<float>();
    }

    private void OnTurn(InputValue value)
    {
        if (!GameStartPressed) // to prevent player from moving before exiting start screen
            return;

        _turnInput = value.Get<float>();
    }

    private void OnStartGame(InputValue value)
    {
        if (GameStartPressed)
            return;

        GameStartPressed = true;
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