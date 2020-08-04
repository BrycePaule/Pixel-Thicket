using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private GameEventSystem _gameEventSystem;

    public Vector2 PlayerMoveAxis;
    public Vector2 MousePos;

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _gameEventSystem = GameEventSystem.Instance;

        PlayerInput.MouseActions Mouse = _playerInput.Mouse;
        PlayerInput.MovementActions Movement = _playerInput.Movement;
        PlayerInput.AttackActions Attack = _playerInput.Attack;
        PlayerInput.UIActions UI = _playerInput.UI;

        Mouse.Position.performed += ctx => OnMouseMove(ctx.ReadValue<Vector2>());

        Movement.Move.performed += ctx => OnPlayerMove(ctx.ReadValue<Vector2>());
        Movement.Move.canceled += ctx => OnPlayerStopMove();

        Movement.Sprint.performed += ctx => OnSprintPress();
        Movement.Sprint.canceled += ctx => OnSprintRelease();

        Movement.Dash.performed += ctx => OnDashPress();

        Attack.Shoot.performed += ctx => OnShootPress();

        UI.Inventory.performed += ctx => OnInventoryPress();
    }

    // EVENTS
    private void OnMouseMove(Vector2 context)
    {
        MousePos = context;
    }

    private void OnPlayerMove(Vector2 context)
    {
        PlayerMoveAxis = context;
    }

    private void OnPlayerStopMove()
    {
        PlayerMoveAxis = Vector2.zero;
    }

    private void OnSprintPress()
    {
        _gameEventSystem.OnSprintPress();
    }

    private void OnSprintRelease()
    {
        _gameEventSystem.OnSprintRelease();
    }

    private void OnDashPress()
    {
        _gameEventSystem.OnDashPress();
    }

    private void OnInventoryPress()
    {
        _gameEventSystem.OnInventoryPress();
    }

    private void OnShootPress()
    {
        _gameEventSystem.OnShootPress(MousePos);
    }

}