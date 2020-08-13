using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : InventorySlot
{
    [SerializeField] private Image _selectImage;

    public HotbarUI HotbarUI;

    public void Select()
    {
        _selectImage.enabled = true;
        HotbarUI.ChangeSelection(SlotNumber);
    }

    public void Deselect()
    {
        _selectImage.enabled = false;
    }
}
