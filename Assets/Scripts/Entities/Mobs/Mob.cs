using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
// 
// 
// 
// 
// pads in base are shop for extra stats
// other one is shop for upgrading mob level etc - at different mob levels you get different kinds of monsters - upgraded slimes etc
//     maybe buy upgrades to different spells and stuff
// end room in the maze is a platform
// make every mobs soul light it up (give everything a light) - means mobs can drop soul, that you spend to light up the world or some shit idk
//     means you can make the mood of the game darker, and add light sources throughout the game with new sprite
// 
// 
// 
// NOTES FROM SHOWER ^^^^^^^^^^^^^
// 
// 
// 
// 
// 
// 
// 

public class Mob : MonoBehaviour
{

    [Space(10)]
    [SerializeField] private float _damage;

    public bool Aggro;
    public bool Hit;

    protected Transform _transform;
    protected Animator _animator;

    // UNITY METHODS
    protected virtual void Awake() 
    {
        _transform = transform;

        _animator = GetComponent<Animator>();
    }

    protected virtual void Start() 
    {

    }

    // COLLISION
    private void OnCollisionEnter2D(Collision2D other) 
    {
        GetComponentInChildren<MobMovement>()._destArrived = true;

        if (other.gameObject.layer == 12) 
        {
            IDamageable<float> target = other.transform.GetComponentInChildren<IDamageable<float>>();
            if (target == null) { return; }

            target.Damage(_damage);
        } 
    }

    // CALLBACKS
    private void ShootFinish()
    {
        _animator.ResetTrigger("Shoot");
    }

    private void HitFinish()
    {
        _animator.ResetTrigger("Hit");
        Hit = false;
    }

    private void HopEnd()
    {
        GetComponentInChildren<MobMovement>().StartCoroutine("HopEnd");
    }

}
