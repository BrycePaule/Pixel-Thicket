using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [SerializeField] private InventoryUI _inventoryUI; 
    [SerializeField] protected Image _itemImage; 

    public int SlotNumber;
    public Spell Item;
    public bool _pickedUp;

    private Vector3 _itemImagePosition;
    private Vector3 _itemImagePositionLocal;
    private float pickupXOffset = 20f;
    private float pickupYOffset = -20f;

    private InputManager _inputManager;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
    }

    private void Start()
    {
        _itemImagePosition = _itemImage.transform.position;
        _itemImagePositionLocal = _itemImage.transform.localPosition;
    }

    private void Update() 
    {
        if (_pickedUp)
        {
            ItemFollowMouse();
        }
    }

    public virtual void AddItem(Spell newItem)
    {
        if (newItem == null) { return; }

        Item = newItem;
        _itemImage.sprite = newItem.Icon;
        _itemImage.enabled = true;
    }

    public virtual void ClearSlot()
    {
        Item = null;
        _itemImage.sprite = null;
        _itemImage.enabled = false;
    }

    public void OnClick()
    {
        _inventoryUI.OnSlotClick(SlotNumber);
    }

    private void ItemFollowMouse()
    {
        _itemImage.transform.position = new Vector3(_inputManager.MousePos.x + pickupXOffset, _inputManager.MousePos.y + pickupYOffset, 0f);
    }

    public void ResetItemPosition()
    {
        _pickedUp = false;
        _itemImage.transform.position = _itemImagePosition;
        _itemImage.transform.localPosition = _itemImagePositionLocal;
    }


}
