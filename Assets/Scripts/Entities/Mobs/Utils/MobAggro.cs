using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAggro : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) 
        {
            
            transform.parent.GetComponentInChildren<Mob>().Aggro = true;
            transform.parent.GetComponentInChildren<MobMovement>().MovePattern = MovementPattern.FollowPlayer;
        } 
    }
    
}
