using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [SerializeField] private Image _itemImage; 
    [SerializeField] private Button _removeButton; 

    private RangedAttack item;

    public void AddItem(RangedAttack newItem)
    {
        item = newItem;
        _itemImage.sprite = newItem.Icon;
        _itemImage.enabled = true;
        _removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.enabled = false;
        _removeButton.interactable = false;
    }

    private void OnRemoveButtonClick()
    {

    }
}
