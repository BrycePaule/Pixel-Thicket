using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private RangedAttack Fireball;

    private GameEventSystem _gameEventSystem;

    public List<RangedAttack> Items = new List<RangedAttack>();
    private int slots;

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onZPress += OnZPress;
        _gameEventSystem.onXPress += OnXPress;
    }

    private void FixedUpdate()
    {
        print(Items.Count);
    }

    public void Add(RangedAttack item)
    {
        if (Items.Count < slots) { return; }

        Items.Add(item);
        _gameEventSystem.OnInventoryChanged();
    }

    public void Remove(RangedAttack item)
    {
        Items.Remove(item);
        _gameEventSystem.OnInventoryChanged();
    }

    private void OnZPress() 
    {
        Add(Fireball);
        print("adding fireball");
    }

    private void OnXPress() 
    {
        Remove(Fireball);
        print("removing fireball");
    }
}
