using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player : MonoBehaviour
{
    private GameEventManager _gameEventManager;

    private void Awake()
    {
        _gameEventManager = GameEventManager.Instance;

        _gameEventManager.onXPress += OnXPress;
        _gameEventManager.onZPress += OnZPress;
    }

    // EVENTS   
    private void OnXPress()
    {
        print("Loading player");
        SaveManager.LoadPlayer(SaveManager.LoadPlayerData(), this);
    }
    
    private void OnZPress()
    {
        print("Saving player");
        SaveManager.SavePlayerData(this);
    }
    
}