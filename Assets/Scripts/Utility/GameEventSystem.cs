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

    public event Action<int> onGatewayEnter;

    public void OnGatewayEnter(int direction)
    {
        if (onGatewayEnter == null) { return; }
        onGatewayEnter(direction);
        
    }

}
