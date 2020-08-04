using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameEventSystem _gameEventSystem;

    private List<RangedAttack> _items = new List<RangedAttack>();

    public Inventory()
    {}

    private void Awake()
    {
        _gameEventSystem = GameEventSystem.Instance;

        _gameEventSystem.onInventoryPress += OnInventoryPress;
    }

    // public void OpenInventory() => this.gameObject.SetActive(true);
    // public void CloseInventory() => this.gameObject.SetActive(false);

    public void ToggleInventory()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
        }
    }

    // EVENTS
    private void OnInventoryPress() => ToggleInventory();

}
