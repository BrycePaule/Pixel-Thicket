using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    private Transform _transform;


    private void Awake()
    {
        _transform = transform;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo) 
    {
        if (_transform.IsChildOf(hitInfo.transform)) { return; }
        if (_transform.parent == hitInfo.transform.parent) { return; }

        Destroy(_transform.gameObject);
    }

}
