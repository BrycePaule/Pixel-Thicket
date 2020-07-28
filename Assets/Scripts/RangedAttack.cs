using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{

    public SpriteRenderer SR;

    [Header("These should be null")]
    public RangedAttackSO RangedAttackType;
    public string Name;
    public float Damage;
    public float Cooldown;
    public float MissileSpeed;
    public Transform Shooter;

    // public Sprite Icon;
    // public Sprite Sprite;

    private Transform _transform;


    private void Awake()
    {
        _transform = transform;

        
    }

    private void Start() 
    {
        ParticleSystem[] particles = _transform.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var particle in particles)
        {
            if (particle.name == Name)
            {
                particle.gameObject.SetActive(true);
            }
        }
    }

    public void SetSOData()
    {
        Name = RangedAttackType.Name;
        Damage = RangedAttackType.Damage;
        Cooldown = RangedAttackType.Cooldown;
        MissileSpeed = RangedAttackType.MissileSpeed;

        SR.sprite = RangedAttackType.Sprite;

        if (Shooter.gameObject.layer == 13) {
            _transform.gameObject.layer = 13;
        }
}

    private void OnCollisionEnter2D(Collision2D hitInfo) 
    {
        // print(hitInfo.collider.name);

        if (_transform.IsChildOf(hitInfo.transform)) { return; }
        if (_transform.parent == hitInfo.transform.parent) { return; }

        Destroy(_transform.gameObject);
    }

    // private void OnTriggerEnter2D(Collider2D hitInfo) 
    // {

    //     if (_transform.IsChildOf(hitInfo.transform)) { return; }
    //     if (_transform.parent == hitInfo.transform.parent) { return; }

    //     Destroy(_transform.gameObject);
    // }

}
