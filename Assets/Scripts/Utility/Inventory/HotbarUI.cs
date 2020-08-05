using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{

    [SerializeField] private Inventory _inventory;
    [SerializeField] private HotbarSlot _hotbarSlotPrefab;
    [SerializeField] private Transform _slotContainer;

    private HotbarSlot[] _slots;
    private int _slotCount;

    private GameEventSystem _gameEventSystem;
    private Canvas _hotbarCanvas;

    private int _selected = 0;

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onInventoryChanged += OnInventoryChanged;
        _gameEventSystem.onMouseScroll += context => OnMouseScroll(context);

        PopulateSlots();
    }

    private void Start()
    {
        _slotCount = _inventory.GetComponent<Hotbar>().Slots;
        _slots = _slotContainer.GetComponentsInChildren<HotbarSlot>();
        UpdateHotbar();
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

            _slots[i].Deselect();
        }

        _slots[_selected].Select();
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

    private void OnMouseScroll(float direction)
    {
        if (direction > 0)
        {
            _selected ++;
            if (_selected >= _slotCount) 
            { 
                _selected = 0; 
            }
        }

        else
        {
            _selected --;
            if (_selected < 0) 
            { 
                _selected = _slotCount - 1;
            }
        }

        UpdateHotbar();
    }
}
