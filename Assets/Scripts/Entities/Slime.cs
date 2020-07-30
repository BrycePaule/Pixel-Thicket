using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Color _color;

    [SerializeField] [Range(0, 3)] private float _hopCooldown;

    private int _generation;
    private bool _hopWait;


    // private void Awake()
    // {

    // }

    public override void Start()
    {
        _sr.color = _color;
    }

    private IEnumerator HopWait()
    {
        _hopWait = true;
        yield return new WaitForSeconds(_hopCooldown);
        _hopWait = false;
    }

}