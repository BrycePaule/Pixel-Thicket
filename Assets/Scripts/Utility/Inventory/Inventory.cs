using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int Selected;
    [SerializeField] private RangedAttack Fireball;
    [SerializeField] private RangedAttack Frostbolt;

    private GameEventSystem _gameEventSystem;

    public List<RangedAttack> Items = new List<RangedAttack>();
    private int _slots;

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onZPress += OnZPress;
        _gameEventSystem.onXPress += OnXPress;
    }

    public void Add(RangedAttack item)
    {
        if (Items.Count < _slots) { return; }

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
        Add(Frostbolt);
        print("adding frostbolt");
    }

    public RangedAttack SelectedWeapon()
    {
        if (Items.Count > Selected)
        {
            return Items[Selected];
        }
        else
        {
            return null;
        }
    }
}
