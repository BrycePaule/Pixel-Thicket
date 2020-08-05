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

}
