using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int Selected;
    [SerializeField] private Spell[] Spells;

    private SpellManager _spellManager;
    private GameEventManager _gameEventManager;

    public List<Spell> Items = new List<Spell>();
    private int _slots;

    private int _spellSelector;

    private void Awake()
    {
        _gameEventManager = GameEventManager.Instance;
        _spellManager = SpellManager.Instance;

        _gameEventManager.onSlot1Press += OnSlot1Press;
        _gameEventManager.onSlot2Press += OnSlot2Press;
        _gameEventManager.onSlot3Press += OnSlot3Press;
        _gameEventManager.onSlot4Press += OnSlot4Press;
        _gameEventManager.onSlot5Press += OnSlot5Press;

        _gameEventManager.onZPress += OnZPress;
        _gameEventManager.onXPress += OnXPress;
    }

    public void AddItem(Spell item)
    {
        if (Items.Count < _slots) { return; }

        Items.Add(item);
        _gameEventManager.OnInventoryChanged();
    }

    public void AddItem(int itemID)
    {
        if (Items.Count < _slots) { return; }

        Items.Add(_spellManager.GetSpell(itemID).GetComponent<Spell>());
        _gameEventManager.OnInventoryChanged();
    }

    public void Remove(Spell item)
    {
        Items.Remove(item);
        _gameEventManager.OnInventoryChanged();
    }

    public void ClearInventory()
    {
        Items = new List<Spell>();
        _gameEventManager.OnInventoryChanged();
    }

    public Spell SelectedWeapon()
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

    public void ChangeSelection(int slotNumber)
    {
        if (Selected == slotNumber) { return; }

        Selected = slotNumber;
        _gameEventManager.OnInventoryChanged();
    }

    public void SwapSlots(int slot1, int slot2)
    {
        Spell item1 = (slot1 < Items.Count) ? Items[slot1] : null;
        Spell item2 = (slot2 < Items.Count) ? Items[slot2] : null;

        Items[slot1] = item2;
        Items[slot2] = item1;
    }

    // EVENTS
    private void OnZPress() 
    {
        AddItem(Spells[_spellSelector]);
        _spellSelector = (_spellSelector >= Spells.Length - 1) ? 0 : _spellSelector + 1; 
    }

    private void OnXPress() 
    {
        // Add(Attacks[1]);
        // print("adding XXXX");
    }

    private void OnSlot1Press() => ChangeSelection(0);
    private void OnSlot2Press() => ChangeSelection(1);
    private void OnSlot3Press() => ChangeSelection(2);
    private void OnSlot4Press() => ChangeSelection(3);
    private void OnSlot5Press() => ChangeSelection(4);
}
