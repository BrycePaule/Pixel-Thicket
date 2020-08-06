using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private GameEventSystem _gameEventSystem;

    public Vector2 PlayerMoveAxis;
    public Vector2 MousePos;

    private void OnEnable() => EnableControls();
    private void OnDisable() => DisableControls();

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _gameEventSystem = GameEventSystem.Instance;

        PlayerInput.MouseActions Mouse = _playerInput.Mouse;
        PlayerInput.MovementActions Movement = _playerInput.Movement;
        PlayerInput.AttackActions Attack = _playerInput.Attack;
        PlayerInput.UIActions UI = _playerInput.UI;
        PlayerInput.DevKeyActions DevKey = _playerInput.DevKey;

        Mouse.Position.performed += ctx => OnMouseMove(ctx.ReadValue<Vector2>());
        Mouse.Scroll.performed += ctx => OnScroll(ctx.ReadValue<Vector2>());

        Movement.Move.performed += ctx => OnPlayerMove(ctx.ReadValue<Vector2>());
        Movement.Move.canceled += ctx => OnPlayerStopMove();

        Movement.Sprint.performed += ctx => OnSprintPress();
        Movement.Sprint.canceled += ctx => OnSprintRelease();

        Movement.Dash.performed += ctx => OnDashPress();

        Attack.Shoot.performed += ctx => OnShootPress();

        UI.Inventory.performed += ctx => OnInventoryPress();
        UI.Slot1.performed += ctx => OnSlot1Press();
        UI.Slot2.performed += ctx => OnSlot2Press();
        UI.Slot3.performed += ctx => OnSlot3Press();
        UI.Slot4.performed += ctx => OnSlot4Press();
        UI.Slot5.performed += ctx => OnSlot5Press();

        // DEV KEYS
        DevKey.AddItem.performed += ctx => OnZPress();
        DevKey.RemoveItem.performed += ctx => OnXPress();
    }

    public void DisableControls()
    {
        _playerInput.Disable();
    }

    public void EnableControls()
    {
        _playerInput.Enable();
    }

    // EVENTS
    private void OnMouseMove(Vector2 context)
    {
        MousePos = context;
    }

    private void OnScroll(Vector2 context)
    {
        _gameEventSystem.OnMouseScroll(context.y);
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

    private void OnSlot1Press()
    {
        _gameEventSystem.OnSlot1Press();
    }

    private void OnSlot2Press()
    {
        _gameEventSystem.OnSlot2Press();
    }

    private void OnSlot3Press()
    {
        _gameEventSystem.OnSlot3Press();
    }

    private void OnSlot4Press()
    {
        _gameEventSystem.OnSlot4Press();
    }

    private void OnSlot5Press()
    {
        _gameEventSystem.OnSlot5Press();
    }

    private void OnShootPress()
    {
        _gameEventSystem.OnShootPress(MousePos);
    }

    // DEV KEYS
    private void OnZPress()
    {
        _gameEventSystem.OnZPress();
    }

    private void OnXPress()
    {
        _gameEventSystem.OnXPress();
    }
    
}