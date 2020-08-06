using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : InventorySlot
{
    [SerializeField] private Image _selectImage;

    public override void AddItem(RangedAttack newItem)
    {
        item = newItem;
        _itemImage.sprite = newItem.Icon;
        _itemImage.enabled = true;
    }

    public override void ClearSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.enabled = false;
    }

    public void Select()
    {
        _selectImage.enabled = true;
    }

    public void Deselect()
    {
        _selectImage.enabled = false;
    }
}
