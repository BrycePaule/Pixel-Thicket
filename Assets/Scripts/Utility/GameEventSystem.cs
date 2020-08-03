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

    public void OnGatewayEnter(int direction)
    {
        if (onGatewayEnter == null) { return; }
        onGatewayEnter(direction);
    }

    public void OnPlayerDeath()
    {
        if (onPlayerDeath == null) { return; }
        onPlayerDeath();
    }
}
