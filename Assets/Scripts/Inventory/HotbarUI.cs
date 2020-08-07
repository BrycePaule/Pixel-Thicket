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

    private GameEventManager _gameEventManager;
    private Canvas _hotbarCanvas;

    private int _selected = 0;

    private void Awake()
    {
        _gameEventManager = GameEventManager.Instance;

        _gameEventManager.onInventoryChanged += OnInventoryChanged;
        _gameEventManager.onMouseScroll += context => OnMouseScroll(context);
        _gameEventManager.onSlot1Press += OnSlot1Press;
        _gameEventManager.onSlot2Press += OnSlot2Press;
        _gameEventManager.onSlot3Press += OnSlot3Press;
        _gameEventManager.onSlot4Press += OnSlot4Press;
        _gameEventManager.onSlot5Press += OnSlot5Press;

        PopulateSlots();
    }

    private void Start()
    {    
        _slots = _slotContainer.GetComponentsInChildren<HotbarSlot>();
        _slotCount = _slots.Length;
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SlotNumber = i;
            _slots[i].HotbarUI = this;
        }

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

    public void ChangeSelection(int selectedSlot)
    {
        if (_selected == selectedSlot) { return; }

        _selected = selectedSlot;
        _inventory.Selected = _selected;
        _gameEventManager.OnInventoryChanged();
    }

    private void OnInventoryChanged() => UpdateHotbar();

    private void OnMouseScroll(float direction)
    {
        if (direction > 0)
        {
            _selected++;
            if (_selected >= _slotCount) 
            { 
                _selected = 0; 
            }
        }

        else
        {
            _selected--;
            if (_selected < 0) 
            { 
                _selected = _slotCount - 1;
            }
        }

        UpdateHotbar();
    }

    private void OnSlot1Press() => ChangeSelection(0);
    private void OnSlot2Press() => ChangeSelection(1);
    private void OnSlot3Press() => ChangeSelection(2);
    private void OnSlot4Press() => ChangeSelection(3);
    private void OnSlot5Press() => ChangeSelection(4);
}
