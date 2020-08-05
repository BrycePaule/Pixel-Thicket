using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public int Slots;
    public int _selected = 0;

    [SerializeField] Inventory _inventory;

    private GameEventSystem _gameEventSystem;
    

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onMouseScroll += context => OnMouseScroll(context);
        _gameEventSystem.onSlot1Press += OnSlot1Press;
        _gameEventSystem.onSlot2Press += OnSlot2Press;
        _gameEventSystem.onSlot3Press += OnSlot3Press;
        _gameEventSystem.onSlot4Press += OnSlot4Press;
        _gameEventSystem.onSlot5Press += OnSlot5Press;
    }

    private void ChangeSelection(int selectedSlot)
    {
        if (_selected == selectedSlot) { return; }

        _selected = selectedSlot;
        _gameEventSystem.OnInventoryChanged();
    }

    private void OnMouseScroll(float direction)
    {
        if (direction > 0)
        {
            _selected ++;
            if (_selected >= Slots) 
            { 
                _selected = 0; 
            }
        }

        else
        {
            _selected --;
            if (_selected < 0) 
            { 
                _selected = Slots - 1;
            }
        }

        _inventory.Selected = _selected;
    }

    private void OnSlot1Press() => ChangeSelection(0);
    private void OnSlot2Press() => ChangeSelection(1);
    private void OnSlot3Press() => ChangeSelection(2);
    private void OnSlot4Press() => ChangeSelection(3);
    private void OnSlot5Press() => ChangeSelection(4);
}
