using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [SerializeField] protected Image _itemImage; 

    public int SlotNumber;
    private bool _selected;

    protected Spell item;

    private void FixedUpdate()
    {

    }

    public virtual void AddItem(Spell newItem)
    {
        item = newItem;
        _itemImage.sprite = newItem.Icon;
        _itemImage.enabled = true;
    }

    public virtual void ClearSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.enabled = false;
    }


}
