using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public Animator animator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    private bool sprinting = false;

    private Vector2 moveAxis;
    private float hSpeed;
    private float vSpeed;

    void OnEnable(){ PlayerInput.Enable(); }
    void OnDisable(){ PlayerInput.Disable(); }

    void Awake()
    {
        PlayerInput = new PlayerInput();
        // PlayerInput.Movement.Move.performed += context => OnMoveInput(context);
    }

    void Update()
    {
        moveAxis = PlayerInput.Movement.Move.ReadValue<Vector2>();
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
        if (sprinting)
        {
            transform.position += new Vector3(moveAxis.x * moveSpeed * sprintMultiplier * Time.deltaTime, moveAxis.y * moveSpeed * sprintMultiplier * Time.deltaTime, 0f);
        }
        else
        {
            transform.position += new Vector3(moveAxis.x * moveSpeed * Time.deltaTime, moveAxis.y * moveSpeed * Time.deltaTime, 0f);
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
}
