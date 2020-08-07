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
        _slots = _inventoryContainer.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SlotNumber = i;
        }

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

    // EVENTS
    private void OnInventoryPress() => ToggleInventory();
    private void OnInventoryChanged() => UpdateInventoryUI();
}
