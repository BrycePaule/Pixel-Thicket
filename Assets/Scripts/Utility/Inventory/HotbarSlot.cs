using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : InventorySlot
{
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
}
