using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    private static GameEventSystem _instance;

    public static GameEventSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameEventSystem>();
                if (_instance == null)
                {
                    _instance = new GameEventSystem();
                }
            }
            return _instance;
        }
    }

    private void Awake() 
    {
        if (_instance != null) { Destroy(this.gameObject); }
    }

    // EVENTS
    public event Action<int> onGatewayEnter;

    public event Action onSprintPress;
    public event Action onSprintRelease;
    public event Action onDashPress;
    public event Action<Vector2> onShootPress;
    public event Action onPlayerDeath;

    public event Action onInventoryPress;
    public event Action onInventoryChanged;

    public event Action onSlot1Press;
    public event Action onSlot2Press;
    public event Action onSlot3Press;
    public event Action onSlot4Press;
    public event Action onSlot5Press;

    public event Action<float> onMouseScroll;

    // DEV KEYS
    public event Action onZPress;
    public event Action onXPress;

    public void OnGatewayEnter(int direction)
    {
        if (onGatewayEnter == null) { return; }
        onGatewayEnter(direction);
    }

    public void OnSprintPress()
    {
        if (onDashPress == null) { return; }
        onSprintPress();
    }

    public void OnSprintRelease()
    {
        if (onDashPress == null) { return; }
        onSprintRelease();
    }

    public void OnPlayerDeath()
    {
        if (onPlayerDeath == null) { return; }
        onPlayerDeath();
    }

    public void OnDashPress()
    {
        if (onDashPress == null) { return; }
        onDashPress();
    }

    public void OnShootPress(Vector2 mousePos)
    {
        if (onDashPress == null) { return; }
        onShootPress(mousePos);
    }

    public void OnInventoryPress()
    {
        if (onInventoryPress == null) { return; }
        onInventoryPress();
    }

    public void OnSlot1Press()
    {
        if (onSlot1Press == null) { return; }
        onSlot1Press();
    }

    public void OnSlot2Press()
    {
        if (onSlot2Press == null) { return; }
        onSlot2Press();
    }

    public void OnSlot3Press()
    {
        if (onSlot3Press == null) { return; }
        onSlot3Press();
    }

    public void OnSlot4Press()
    {
        if (onSlot4Press == null) { return; }
        onSlot4Press();
    }

    public void OnSlot5Press()
    {
        if (onSlot5Press == null) { return; }
        onSlot5Press();
    }

    public void OnInventoryChanged()
    {
        if (onDashPress == null) { return; }
        onInventoryChanged();
    }

    public void OnMouseScroll(float value)
    {
        if (onMouseScroll == null) { return; }
        onMouseScroll(value);
    }

    public void OnZPress()
    {
        if (onZPress == null) { return; }
        onZPress();
    }

    public void OnXPress()
    {
        if (onXPress == null) { return; }
        onXPress();
    }


}
