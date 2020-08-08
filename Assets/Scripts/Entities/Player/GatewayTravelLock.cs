using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayTravelLock : MonoBehaviour, IGatewayTravelLockable
{
    [SerializeField] private float _lockTime;

    [HideInInspector] public bool _locked;

    private GameEventManager _gameEventManager;

    private void Awake()
    {
        _gameEventManager = GameEventManager.Instance;
    }

    public void LockGatewayTravel(CardinalDirection direction)
    {
        if (_locked) { return; }

        _gameEventManager.OnGatewayEnter(direction);
       StartCoroutine(LockTimer());
    }

    private IEnumerator LockTimer()
    {
        _locked = true;

        yield return new WaitForSeconds(_lockTime);

        _locked = false;
    }
}
