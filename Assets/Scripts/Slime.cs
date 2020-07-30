using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Color _color;

    private int _generation;


    // private void Awake()
    // {

    // }

    public override void Start()
    {
        _sr.color = _color;
    }

    // public override void Move()
    // {

    // }

}