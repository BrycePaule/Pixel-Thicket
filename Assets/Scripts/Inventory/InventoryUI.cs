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
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _inventory.Items.Count)
            {
                _slots[i].AddItem(_inventory.Items[i]);
            }
            else
            {
                _slots[i].ClearSlot();
            }
        }
    }

    public void ToggleInventory() => _inventoryCanvas.enabled = !_inventoryCanvas.enabled;

    public void OnSlotClick(int slotNumber)
    {
        if (_selectedSlot == -1)
        {
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

    // EVENTS
    private void OnInventoryPress() => ToggleInventory();
    private void OnInventoryChanged() => UpdateInventoryUI();

}
