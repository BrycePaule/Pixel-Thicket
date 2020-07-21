using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : RangedAttack
{

    private Transform _transform;


    private void Awake()
    {
        _transform = transform;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo) 
    {
        if (!_transform.IsChildOf(hitInfo.transform))
        {
            // print(hitInfo.name);
            // Destroy(hitInfo.gameObject);
            Destroy(this);
        }
    }
}
