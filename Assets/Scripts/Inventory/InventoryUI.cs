using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;

    private GameEventSystem _gameEventSystem;
    private Canvas _inventoryCanvas;
    private Transform _inventoryContainer;

    private InventorySlot[] _slots;

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;
        _inventoryCanvas = GetComponentInParent<Canvas>();
        _inventoryContainer = transform;

        _gameEventSystem.onInventoryPress += OnInventoryPress;
        _gameEventSystem.onInventoryChanged += OnInventoryChanged;
    }

    private void Start()
    {
        _slots = _inventoryContainer.GetComponentsInChildren<InventorySlot>();
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
