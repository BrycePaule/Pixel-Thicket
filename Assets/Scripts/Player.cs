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
    public ParticleSystem _dashParticle;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    private float _sprinting;
    private float _dashPress;
    private float dashTimeCounter;
    private bool _dashing = false;

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
        _sprinting = _playerInput.Movement.Sprint.ReadValue<float>();
        _dashPress = _playerInput.Movement.Dash.ReadValue<float>();

        animator.SetFloat("Horizontal", _moveAxis.x);
        animator.SetFloat("Vertical", _moveAxis.y);
        animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(_moveAxis.x), Mathf.Abs(_moveAxis.y)));
    }

    void FixedUpdate() {
        
        if (_dashPress != 0 & _dashing == false)
        {
            dashTimeCounter = dashTime;
            _dashing = true;
            Dash();
        }

        if (_dashing == true)
        {
            dashTimeCounter -= Time.deltaTime;
            _dashParticle.Play();

            if (dashTimeCounter <= 0)
            {
                print("ending dash");
                rb.velocity = new Vector2(0, 0);
                _dashing = false;
            }
        }

        if (!_dashing) {
            if (_moveAxis != new Vector2(0f, 0f))
            {
                Move();
                _leafParticle.Play();
            }
        }
        
    }

    // PLAYER MOVEMENT
    void Move()
    {   
        float timeDelta = Time.deltaTime;

        if (_sprinting != 0f)
        {
            _transform.position += new Vector3(_moveAxis.x * moveSpeed * sprintMultiplier * timeDelta, _moveAxis.y * moveSpeed * sprintMultiplier * timeDelta, 0f);
            // rb.AddForce(new Vector2(_moveAxis.x * moveSpeed * sprintMultiplier * timeDelta, _moveAxis.y * moveSpeed * sprintMultiplier * timeDelta), ForceMode2D.Impulse);
        }
        else
        {
            _transform.position += new Vector3(_moveAxis.x * moveSpeed * timeDelta, _moveAxis.y * moveSpeed * timeDelta, 0f);
            // rb.AddForce(new Vector2(_moveAxis.x * moveSpeed * sprintMultiplier * timeDelta, _moveAxis.y * moveSpeed * sprintMultiplier * timeDelta), ForceMode2D.Impulse);
        }
    }

    void Dash()
    {
        rb.AddForce(new Vector2(_moveAxis.x * dashSpeed, _moveAxis.y * dashSpeed), ForceMode2D.Impulse);
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveAxis = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // print(other.name);
    }
}
