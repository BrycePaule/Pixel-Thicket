using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private PlayerInput _playerInput;
    private GameEventManager _gameEventManager;

    public Vector2 PlayerMoveAxis;
    public Vector2 MousePos;
    public Vector3 MousePosWorld;

    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                {
                    _instance = new InputManager();
                }
            }
            return _instance;
        }
    }

    private void OnEnable() => EnableControls();
    private void OnDisable() => DisableControls();

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _gameEventManager = GameEventManager.Instance;

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

    private void ConvertMousePosToWorld(Vector2 mousePos)
    {
            MousePosWorld = _camera.ScreenToWorldPoint(mousePos);
            MousePosWorld.z = 0f;
    }

    // EVENTS
    private void OnMouseMove(Vector2 context)
    {
        MousePos = context;
        ConvertMousePosToWorld(context);
    }

    private void OnScroll(Vector2 context)
    {
        _gameEventManager.OnMouseScroll(context.y);
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
        _gameEventManager.OnSprintPress();
    }

    private void OnSprintRelease()
    {
        _gameEventManager.OnSprintRelease();
    }

    private void OnDashPress()
    {
        _gameEventManager.OnDashPress();
    }

    private void OnInventoryPress()
    {
        _gameEventManager.OnInventoryPress();
    }

    private void OnSlot1Press()
    {
        _gameEventManager.OnSlot1Press();
    }

    private void OnSlot2Press()
    {
        _gameEventManager.OnSlot2Press();
    }

    private void OnSlot3Press()
    {
        _gameEventManager.OnSlot3Press();
    }

    private void OnSlot4Press()
    {
        _gameEventManager.OnSlot4Press();
    }

    private void OnSlot5Press()
    {
        _gameEventManager.OnSlot5Press();
    }

    private void OnShootPress()
    {
        _gameEventManager.OnShootPress(MousePos);
    }

    // DEV KEYS
    private void OnZPress()
    {
        _gameEventManager.OnZPress();
    }

    private void OnXPress()
    {
        _gameEventManager.OnXPress();
    }
    
}