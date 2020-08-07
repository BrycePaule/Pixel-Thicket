using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEventManager : MonoBehaviour
{
    private static GameEventManager _instance;

    public static GameEventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameEventManager>();
                if (_instance == null)
                {
                    _instance = new GameEventManager();
                }
            }
            return _instance;
        }
    }

    private void Awake() 
    {
        if (_instance != null) { Destroy(gameObject); print(_instance); }
    }

    // EVENTS
    public event Action<int> onGatewayEnter;

    public event Action onSprintPress;
    public event Action onSprintRelease;
    public event Action onDashPress;
    public event Action<Vector2> onShootPress;
    public event Action onPlayerDeath;

    public event Action onInventoryPress;
    public event Action onInventoryChanged;

    public event Action onSlot1Press;
    public event Action onSlot2Press;
    public event Action onSlot3Press;
    public event Action onSlot4Press;
    public event Action onSlot5Press;

    public event Action<float> onMouseScroll;

    // DEV KEYS
    public event Action onZPress;
    public event Action onXPress;

    public void OnGatewayEnter(int direction)
    {
        if (onGatewayEnter == null) { return; }
        onGatewayEnter(direction);
    }

    public void OnSprintPress()
    {
        if (onDashPress == null) { return; }
        onSprintPress();
    }

    public void OnSprintRelease()
    {
        if (onDashPress == null) { return; }
        onSprintRelease();
    }

    public void OnPlayerDeath()
    {
        if (onPlayerDeath == null) { return; }
        onPlayerDeath();
    }

    public void OnDashPress()
    {
        if (onDashPress == null) { return; }
        onDashPress();
    }

    public void OnShootPress(Vector2 mousePos)
    {
        if (onShootPress == null) { return; }

        if (!IsPointerOverUIElement(mousePos))
        {
            onShootPress(mousePos);
        }

    }

    public void OnInventoryPress()
    {
        if (onInventoryPress == null) { return; }
        onInventoryPress();
    }

    public void OnSlot1Press()
    {
        if (onSlot1Press == null) { return; }
        onSlot1Press();
    }

    public void OnSlot2Press()
    {
        if (onSlot2Press == null) { return; }
        onSlot2Press();
    }

    public void OnSlot3Press()
    {
        if (onSlot3Press == null) { return; }
        onSlot3Press();
    }

    public void OnSlot4Press()
    {
        if (onSlot4Press == null) { return; }
        onSlot4Press();
    }

    public void OnSlot5Press()
    {
        if (onSlot5Press == null) { return; }
        onSlot5Press();
    }

    public void OnInventoryChanged()
    {
        if (onDashPress == null) { return; }
        onInventoryChanged();
    }

    public void OnMouseScroll(float value)
    {
        if (onMouseScroll == null) { return; }
        onMouseScroll(value);
    }

    public void OnZPress()
    {
        if (onZPress == null) { return; }
        onZPress();
    }

    public void OnXPress()
    {
        if (onXPress == null) { return; }
        onXPress();
    }

    // MOUSEOVER CALCULATIONS
    public bool IsPointerOverUIElement(Vector2 mousePos)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(mousePos));
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    ///Gets all event systen raycast results of current mouse or touch position.
    private List<RaycastResult> GetEventSystemRaycastResults(Vector2 mousePos)
    {   
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePos;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }

}
