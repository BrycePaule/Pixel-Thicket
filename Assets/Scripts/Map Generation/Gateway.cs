﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{

    public Direction direction;

    private SceneLoader _sceneLoader;
    private GameEventManager _gameEventManager;
    private Player _player;


    private void Awake()
    {
        _sceneLoader = SceneLoader.Instance;
        _gameEventManager = GameEventManager.Instance;
        _player = FindObjectOfType<Player>();
    }

    public enum Direction 
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 12)
        {
            if (_player.GatewayTravelLocked) { return; }

            _player.StartCoroutine("GatewayLock");
            _gameEventManager.OnGatewayEnter((int)direction);
        }
    }
}
