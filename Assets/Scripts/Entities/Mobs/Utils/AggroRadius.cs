using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRadius : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) 
        {
            transform.parent.GetComponentInChildren<MobMovement>().MovePattern = MovementPattern.FollowPlayer;
        } 
    }
    
}
