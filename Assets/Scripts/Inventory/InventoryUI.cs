using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;

    private GameEventManager _gameEventManager;
    private Canvas _inventoryCanvas;
    private Transform _inventoryContainer;

    private InventorySlot[] _slots;
    private int _selectedSlot = -1;

    private void Awake()
    {
        _gameEventManager = GameEventManager.Instance;
        _inventoryCanvas = GetComponentInParent<Canvas>();
        _inventoryContainer = transform;

        _gameEventManager.onInventoryPress += OnInventoryPress;
        _gameEventManager.onInventoryChanged += OnInventoryChanged;
    }

    private void Start()
    {
        NumberAllSlots();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < _inventory.SlotCount; i++)
        {
            _slots[i].ClearSlot();
            _slots[i].AddItem(_inventory.Items[i]);
        }
    }

    public void ToggleInventory()
    {
        if (_inventoryCanvas.enabled == true)
        {
            ResetSelectedSlot();
            ResetHeldItem();
        }
        
        _inventoryCanvas.enabled = !_inventoryCanvas.enabled;
    } 
        

    public void OnSlotClick(int slotNumber)
    {
        if (_selectedSlot == -1)
        {
            if (_slots[slotNumber].Item == null) { return; }

            _selectedSlot = slotNumber;
            _slots[_selectedSlot]._pickedUp = true;
        }

        else if (_selectedSlot != -1)
        {
            // if (_selectedSlot == slotNumber) 
            // { 
            //     return; 
            // }

            _inventory.SwapSlots(_selectedSlot, slotNumber);
            _slots[_selectedSlot].ResetItemPosition();
            _slots[slotNumber].ResetItemPosition();

            _gameEventManager.OnInventoryChanged();
            ResetSelectedSlot();
        }
    }

    public void ResetSelectedSlot()
    {
        _selectedSlot = -1;
    }

    private void NumberAllSlots()
    {
        _slots = _inventoryContainer.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SlotNumber = i;
        }
    }

    private void ResetHeldItem()
    {
        foreach (InventorySlot slot in _slots)
        {
            slot.ResetItemPosition();
        }
    }

    // EVENTS
    private void OnInventoryPress() => ToggleInventory();
    private void OnInventoryChanged() => UpdateInventoryUI();

}
