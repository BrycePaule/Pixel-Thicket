using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class RangedAttackSO : ScriptableObject
{

    public string Name;
    public int ID;
    public float Damage;
    public float Cooldown;
    public float MissileSpeed;
    public bool Pierce;

    public Sprite Icon;
    public Sprite Sprite;

}
