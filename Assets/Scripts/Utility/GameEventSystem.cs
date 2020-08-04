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
    public event Action onPlayerDeath;
    // public event Action onMouseMove;
    // public event Action onPlayerMove;
    // public event Action onPlayerStopMove;
    public event Action onSprintPress;
    public event Action onSprintRelease;
    public event Action onDashPress;
    public event Action<Vector2> onShootPress;
    public event Action onInventoryPress;

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

    public void OnInventoryPress()
    {
        if (onDashPress == null) { return; }
        onInventoryPress();
    }

    public void OnShootPress(Vector2 mousePos)
    {
        if (onDashPress == null) { return; }
        onShootPress(mousePos);
    }
}
