using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;
    [SerializeField] private HotbarSlot _hotbarSlotPrefab;
    [SerializeField] private Transform _slotContainer;

    [SerializeField] private int _slotCount;
    private HotbarSlot[] _slots;

    private GameEventSystem _gameEventSystem;
    private Canvas _hotbarCanvas;

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onInventoryChanged += OnInventoryChanged;

        PopulateSlots();
    }

    private void Start()
    {
        _slots = _slotContainer.GetComponentsInChildren<HotbarSlot>();
    }

    private void UpdateHotbar()
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

    private void PopulateSlots()
    {

        int diff = GetComponentsInChildren<HotbarSlot>().Length;

        for (int i = 0; i < _slotCount - diff; i++)
        {
            HotbarSlot slot = Instantiate(_hotbarSlotPrefab);
            slot.transform.SetParent(_slotContainer);
        }
    }

    private void OnInventoryChanged() => UpdateHotbar();

}
