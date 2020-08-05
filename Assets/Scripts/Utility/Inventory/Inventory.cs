using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int Selected;
    [SerializeField] private RangedAttack[] Attacks;

    private GameEventSystem _gameEventSystem;

    public List<RangedAttack> Items = new List<RangedAttack>();
    private int _slots;

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onSlot1Press += OnSlot1Press;
        _gameEventSystem.onSlot2Press += OnSlot2Press;
        _gameEventSystem.onSlot3Press += OnSlot3Press;
        _gameEventSystem.onSlot4Press += OnSlot4Press;
        _gameEventSystem.onSlot5Press += OnSlot5Press;

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

    // EVENTS
    private void OnZPress() 
    {
        Add(Attacks[0]);
        print("adding fireball");
    }

    private void OnXPress() 
    {
        Add(Attacks[1]);
        print("adding XXXX");
    }

    private void ChangeSelection(int selectedSlot)
    {
        if (Selected == selectedSlot) { return; }

        Selected = selectedSlot;
        _gameEventSystem.OnInventoryChanged();
    }

    private void OnSlot1Press() => ChangeSelection(0);
    private void OnSlot2Press() => ChangeSelection(1);
    private void OnSlot3Press() => ChangeSelection(2);
    private void OnSlot4Press() => ChangeSelection(3);
    private void OnSlot5Press() => ChangeSelection(4);
}
