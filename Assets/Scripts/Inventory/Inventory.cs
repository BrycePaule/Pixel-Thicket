using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int Selected;
    [SerializeField] private RangedAttack[] Attacks;

    private RangedAttackManager _rangedAttackManager;
    private GameEventManager _gameEventManager;

    public List<RangedAttack> Items = new List<RangedAttack>();
    private int _slots;

    private int _attackSelector = 0;

    private void Awake()
    {
        _gameEventManager = GameEventManager.Instance;
        _rangedAttackManager = RangedAttackManager.Instance;

        _gameEventManager.onSlot1Press += OnSlot1Press;
        _gameEventManager.onSlot2Press += OnSlot2Press;
        _gameEventManager.onSlot3Press += OnSlot3Press;
        _gameEventManager.onSlot4Press += OnSlot4Press;
        _gameEventManager.onSlot5Press += OnSlot5Press;

        _gameEventManager.onZPress += OnZPress;
        _gameEventManager.onXPress += OnXPress;
    }

    public void Add(RangedAttack item)
    {
        if (Items.Count < _slots) { return; }

        Items.Add(item);
        _gameEventManager.OnInventoryChanged();
    }

    public void Add(int itemID)
    {
        if (Items.Count < _slots) { return; }

        Items.Add(_rangedAttackManager.GetAttackByID(itemID).GetComponent<RangedAttack>());
        _gameEventManager.OnInventoryChanged();
    }

    public void Remove(RangedAttack item)
    {
        Items.Remove(item);
        _gameEventManager.OnInventoryChanged();
    }

    public void ClearInventory()
    {
        Items = new List<RangedAttack>();
        _gameEventManager.OnInventoryChanged();
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

    public int[] ItemIDs()
    {
        int[] IDs = new int[Items.Count];

        for (int i = 0; i < Items.Count; i++)
        {
            IDs[i] = Items[i].ID;
        }

        return IDs;
    }

    // EVENTS
    private void OnZPress() 
    {
        Add(Attacks[_attackSelector]);
        _attackSelector = (_attackSelector >= Attacks.Length - 1) ? 0 : _attackSelector + 1; 
    }

    private void OnXPress() 
    {
        // Add(Attacks[1]);
        // print("adding XXXX");
    }

    private void ChangeSelection(int selectedSlot)
    {
        if (Selected == selectedSlot) { return; }

        Selected = selectedSlot;
        _gameEventManager.OnInventoryChanged();
    }

    private void OnSlot1Press() => ChangeSelection(0);
    private void OnSlot2Press() => ChangeSelection(1);
    private void OnSlot3Press() => ChangeSelection(2);
    private void OnSlot4Press() => ChangeSelection(3);
    private void OnSlot5Press() => ChangeSelection(4);
}
