using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player : MonoBehaviour
{
    public PlayerInput _playerInput;
    public Animator animator;
    public Rigidbody2D rb;
    public ParticleSystem _leafParticle;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    private float sprinting;

    private Transform _transform;
    private Vector2 _moveAxis;

    // void OnEnable(){ _playerInput.Enable(); }
    // void OnDisable(){ _playerInput.Disable(); }

    void Awake()
    {
        _transform = transform;
        _playerInput = new PlayerInput();
        _playerInput.Enable();
    }

    void Update()
    {
        _moveAxis = _playerInput.Movement.Move.ReadValue<Vector2>();
        sprinting = _playerInput.Movement.Sprint.ReadValue<float>();

        animator.SetFloat("Horizontal", _moveAxis.x);
        animator.SetFloat("Vertical", _moveAxis.y);
        animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(_moveAxis.x), Mathf.Abs(_moveAxis.y)));
    }

    void FixedUpdate() {
        
        if (_moveAxis != new Vector2(0f, 0f))
        {
            Move();
            _leafParticle.Play();
        }
    }

    // PLAYER MOVEMENT
    void Move()
    {   
        if (sprinting != 0f)
        {
            _transform.position += new Vector3(_moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, _moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime, 0f);
            // rb.AddForce(new Vector2(moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime), ForceMode2D.Impulse);
        }
        else
        {
            _transform.position += new Vector3(_moveAxis.x * moveSpeed * Time.deltaTime, _moveAxis.y * moveSpeed * Time.deltaTime, 0f);
            // rb.AddForce(new Vector2(moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime), ForceMode2D.Impulse);
        }
    }

    void Dash()
    {
        // TODO
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveAxis = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // print(other.name);
    }
}
