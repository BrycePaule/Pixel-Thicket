using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public Animator animator;
    public Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    private float sprinting;

    private Transform _transform;
    private Vector2 moveAxis;
    private float hSpeed;
    private float vSpeed;

    void OnEnable(){ PlayerInput.Enable(); }
    void OnDisable(){ PlayerInput.Disable(); }

    void Awake()
    {
        _transform = transform;
        PlayerInput = new PlayerInput();
        // PlayerInput.Movement.Move.performed += context => OnMoveInput(context);
    }

    void Update()
    {
        moveAxis = PlayerInput.Movement.Move.ReadValue<Vector2>();
        sprinting = PlayerInput.Movement.Sprint.ReadValue<float>();

        animator.SetFloat("Horizontal", moveAxis.x);
        animator.SetFloat("Vertical", moveAxis.y);
        animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(moveAxis.x), Mathf.Abs(moveAxis.y)));
    }

    void FixedUpdate() {
        Move();
    }

    // PLAYER MOVEMENT
    void Move()
    {   
        if (sprinting != 0f)
        {
            _transform.position += new Vector3(moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime, 0f);
            // rb.AddForce(new Vector2(moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime), ForceMode2D.Impulse);
        }
        else
        {
            _transform.position += new Vector3(moveAxis.x * moveSpeed * Time.deltaTime, moveAxis.y * moveSpeed * Time.deltaTime, 0f);
            // rb.AddForce(new Vector2(moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime), ForceMode2D.Impulse);
        }
    }

    void Dash()
    {
        // TODO
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        moveAxis = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // print(other.name);
    }
}
