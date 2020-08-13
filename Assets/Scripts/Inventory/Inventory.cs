using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int SlotCount;
    public Spell[] Items;
    public int Selected;
    [SerializeField] private Spell[] Spells;

    private SpellManager _spellManager;
    private GameEventManager _gameEventManager;


    private int _spellSelector;

    private void Awake()
    {
        Items = new Spell[SlotCount];

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
        for (int i = 0; i < SlotCount; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = item;
                _gameEventManager.OnInventoryChanged();
                break;
            }
        }
    }

    public void AddItem(int itemID)
    {
        if (itemID == -1 ) { return; }

        for (int i = 0; i < SlotCount; i++)
        {
            if (Items[i] == null)
            {
                Spell item = _spellManager.GetSpell(itemID).GetComponent<Spell>();
                Items[i] = item;
                _gameEventManager.OnInventoryChanged();
                break;
            }
        }
    }

    public void Remove(Spell item)
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if (Items[i] == item)
            {
                Items[i] = null;
                _gameEventManager.OnInventoryChanged();
                break;
            }
        }
    }

    public void ClearInventory()
    {
        Items = new Spell[30];
        _gameEventManager.OnInventoryChanged();
    }

    public Spell SelectedWeapon()
    {
        if (Items.Length > Selected)
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
        int[] IDs = new int[SlotCount];

        for (int i = 0; i < SlotCount; i++)
        {
            if (Items[i] == null) 
            { 
                IDs[i] = -1;
            }
            else
            {
                IDs[i] = Items[i].ID;
            }
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
        print("Swapping (" + slot1 + " - " + slot2 + ")");

        Spell item1 = Items[slot1];
        Spell item2 = Items[slot2];

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
