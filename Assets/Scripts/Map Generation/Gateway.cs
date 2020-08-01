using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{

    [SerializeField] public Direction direction;

    private SceneLoader _sceneLoader;
    private GameEventSystem _gameEventSystem;

    private void Awake()
    {
        _sceneLoader = SceneLoader.Instance;
        _gameEventSystem = GameEventSystem.Instance;
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
            _gameEventSystem.OnGatewayEnter((int)direction);
        }

    }
}
