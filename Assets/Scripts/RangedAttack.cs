using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{

    public SpriteRenderer SR;
    public RangedAttackSO RangedAttackType;

    public string Name;
    public float Damage;
    public float Cooldown;
    public Sprite Icon;
    public Sprite Sprite;

    private Transform _transform;


    private void Awake()
    {
        _transform = transform;
    }


    private void Start()
    {
        Name = RangedAttackType.Name;
        Damage = RangedAttackType.Damage;
        Cooldown = RangedAttackType.Cooldown;

        SR.sprite = RangedAttackType.Sprite;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo) 
    {

        if (_transform.IsChildOf(hitInfo.transform)) { return; }
        if (_transform.parent == hitInfo.transform.parent) { return; }

        Destroy(_transform.gameObject);
    }

}
