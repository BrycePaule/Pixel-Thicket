using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : Mob
{

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Color _color;
    
    [SerializeField] [Range(0, 10)] private float _shootCooldown;

    // private void Awake()
    // {

    // }

    protected override void Start()
    {
        _healthBar.SetMaxHealth(Health);

        _sr.color = _color;
    }

}