using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    [SerializeField] private CardinalDirection direction;

    private SceneLoader _sceneLoader;
    private GameEventManager _gameEventManager;
    private Player _player;

    private void Awake()
    {
        _sceneLoader = SceneLoader.Instance;
        _gameEventManager = GameEventManager.Instance;

        _player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        IGatewayTravelLockable lockable = other.transform.GetComponentInChildren<IGatewayTravelLockable>();
        if (lockable == null) { return; }

        lockable.LockGatewayTravel(direction);
    }
}
