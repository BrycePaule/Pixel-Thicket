﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{

    [SerializeField] public Direction direction;

    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = SceneLoader.Instance;
    }

    public enum Direction 
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 12)
        {
            // _sceneLoader.FadeToBlack();
            GameEventSystem.Instance.OnGatewayEnter((int)direction);
        }

    }
}