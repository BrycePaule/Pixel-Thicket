using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRadius : MonoBehaviour
{

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) 
        {
            _transform.parent.GetComponent<Mob>().MovePattern = MovementPattern.FollowPlayer;
        } 
    }
}
