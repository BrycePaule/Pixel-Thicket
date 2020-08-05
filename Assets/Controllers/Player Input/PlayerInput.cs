// GENERATED AUTOMATICALLY FROM 'Assets/Controllers/Player Input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""b6377183-5696-48d1-83df-bbd9047c41ab"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""0b66979e-cfb7-4562-9a91-ac453e15cdf3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""02c8d867-dc2a-43d0-87d8-26f2700a642f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""64d9ef86-a8a8-4508-b3bf-a692323d7a11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""382d41ec-4a42-4dce-9565-2c0294609ffd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""cffef467-6718-4abf-a3eb-ad31a2b4f6e0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""24659148-f625-4c0f-8abf-c2e84086d56d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""603e8aad-61b1-45e4-924f-2856c6f77635"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fc6e6de1-c4b4-4d28-93d8-c69295957469"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""29a2cfbe-5ad2-46a5-8f2d-3db24efd31f6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8d60b07b-bd2f-4261-aa1f-1e67b1c4a15f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Attack"",
            ""id"": ""a632c822-9dea-47ad-a82a-406caadb3fed"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""6dece1c3-cddb-4bec-a73d-88f163c24fb4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3ca62c5b-9e6f-4db1-b971-36941aa787c5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""270bf4cc-fa2b-4c8c-a30b-cfd626bb31bb"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""id"": ""b60ee896-cd29-4cc0-bc52-53c060797da6"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""de805d95-9fe6-4451-9fad-dc78e7e4b9fd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""027fa239-929d-42f1-acc4-a539c3b79e0f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""576866a5-cee4-4a95-8e05-c1ec6652d6b6"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a6af3e5-3d3b-4c26-9ea4-6c3710cf5d6e"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""dda0c799-3b80-4717-98b8-27c029b9ca52"",
            ""actions"": [
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""a9a21670-84d8-4d2d-94a2-37ac25689b81"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot1"",
                    ""type"": ""Button"",
                    ""id"": ""55de114f-d311-4013-90c1-b540dc373405"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot2"",
                    ""type"": ""Button"",
                    ""id"": ""567830a2-465d-49f5-83d7-1772015a1b54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot3"",
                    ""type"": ""Button"",
                    ""id"": ""56b4dfd0-406c-4eba-bfa2-dc702c9f6cef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot4"",
                    ""type"": ""Button"",
                    ""id"": ""48a8d8e9-cafc-41bb-8ff2-50e7b6106d08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slot5"",
                    ""type"": ""Button"",
                    ""id"": ""c35cc99c-0699-4e38-a0bc-f47666c4d40a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cc04f063-33aa-498b-9a51-3b25ad06539f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4763b062-ed79-41ab-8306-f1ce862d5463"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bf8b32c-f8d3-437c-ae21-7d49841a12ac"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7554395-948a-4aa4-8112-ccde834dd3f4"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""196b7a42-9f2d-490a-a806-725b251e3e65"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b3a8a66-7214-48dc-9a34-b3cac198f8bd"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slot5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DevKey"",
            ""id"": ""dc4ce275-0573-4a81-b60b-d3519df87f1e"",
            ""actions"": [
                {
                    ""name"": ""AddItem"",
                    ""type"": ""Button"",
                    ""id"": ""74085d5a-2245-4792-85f2-3c1b8e1ebe05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveItem"",
                    ""type"": ""Button"",
                    ""id"": ""d3bb8d6f-3e99-4fcf-966e-1ce8d2aa4421"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fd67b5eb-7776-46ef-9ca3-50780f462fe7"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AddItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab2e7cfa-7d7c-4672-9950-ff21aa940a77"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RemoveItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_Dash = m_Movement.FindAction("Dash", throwIfNotFound: true);
        m_Movement_Sprint = m_Movement.FindAction("Sprint", throwIfNotFound: true);
        // Attack
        m_Attack = asset.FindActionMap("Attack", throwIfNotFound: true);
        m_Attack_Shoot = m_Attack.FindAction("Shoot", throwIfNotFound: true);
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_Position = m_Mouse.FindAction("Position", throwIfNotFound: true);
        m_Mouse_Scroll = m_Mouse.FindAction("Scroll", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Inventory = m_UI.FindAction("Inventory", throwIfNotFound: true);
        m_UI_Slot1 = m_UI.FindAction("Slot1", throwIfNotFound: true);
        m_UI_Slot2 = m_UI.FindAction("Slot2", throwIfNotFound: true);
        m_UI_Slot3 = m_UI.FindAction("Slot3", throwIfNotFound: true);
        m_UI_Slot4 = m_UI.FindAction("Slot4", throwIfNotFound: true);
        m_UI_Slot5 = m_UI.FindAction("Slot5", throwIfNotFound: true);
        // DevKey
        m_DevKey = asset.FindActionMap("DevKey", throwIfNotFound: true);
        m_DevKey_AddItem = m_DevKey.FindAction("AddItem", throwIfNotFound: true);
        m_DevKey_RemoveItem = m_DevKey.FindAction("RemoveItem", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Dash;
    private readonly InputAction m_Movement_Sprint;
    public struct MovementActions
    {
        private @PlayerInput m_Wrapper;
        public MovementActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @Dash => m_Wrapper.m_Movement_Dash;
        public InputAction @Sprint => m_Wrapper.m_Movement_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Dash.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnDash;
                @Sprint.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Attack
    private readonly InputActionMap m_Attack;
    private IAttackActions m_AttackActionsCallbackInterface;
    private readonly InputAction m_Attack_Shoot;
    public struct AttackActions
    {
        private @PlayerInput m_Wrapper;
        public AttackActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Attack_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Attack; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AttackActions set) { return set.Get(); }
        public void SetCallbacks(IAttackActions instance)
        {
            if (m_Wrapper.m_AttackActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_AttackActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_AttackActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_AttackActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_AttackActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public AttackActions @Attack => new AttackActions(this);

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_Position;
    private readonly InputAction m_Mouse_Scroll;
    public struct MouseActions
    {
        private @PlayerInput m_Wrapper;
        public MouseActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_Mouse_Position;
        public InputAction @Scroll => m_Wrapper.m_Mouse_Scroll;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @Position.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnPosition;
                @Scroll.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnScroll;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Inventory;
    private readonly InputAction m_UI_Slot1;
    private readonly InputAction m_UI_Slot2;
    private readonly InputAction m_UI_Slot3;
    private readonly InputAction m_UI_Slot4;
    private readonly InputAction m_UI_Slot5;
    public struct UIActions
    {
        private @PlayerInput m_Wrapper;
        public UIActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Inventory => m_Wrapper.m_UI_Inventory;
        public InputAction @Slot1 => m_Wrapper.m_UI_Slot1;
        public InputAction @Slot2 => m_Wrapper.m_UI_Slot2;
        public InputAction @Slot3 => m_Wrapper.m_UI_Slot3;
        public InputAction @Slot4 => m_Wrapper.m_UI_Slot4;
        public InputAction @Slot5 => m_Wrapper.m_UI_Slot5;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Inventory.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInventory;
                @Slot1.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot1;
                @Slot1.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot1;
                @Slot1.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot1;
                @Slot2.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot2;
                @Slot2.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot2;
                @Slot2.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot2;
                @Slot3.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot3;
                @Slot3.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot3;
                @Slot3.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot3;
                @Slot4.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot4;
                @Slot4.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot4;
                @Slot4.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot4;
                @Slot5.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot5;
                @Slot5.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot5;
                @Slot5.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSlot5;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @Slot1.started += instance.OnSlot1;
                @Slot1.performed += instance.OnSlot1;
                @Slot1.canceled += instance.OnSlot1;
                @Slot2.started += instance.OnSlot2;
                @Slot2.performed += instance.OnSlot2;
                @Slot2.canceled += instance.OnSlot2;
                @Slot3.started += instance.OnSlot3;
                @Slot3.performed += instance.OnSlot3;
                @Slot3.canceled += instance.OnSlot3;
                @Slot4.started += instance.OnSlot4;
                @Slot4.performed += instance.OnSlot4;
                @Slot4.canceled += instance.OnSlot4;
                @Slot5.started += instance.OnSlot5;
                @Slot5.performed += instance.OnSlot5;
                @Slot5.canceled += instance.OnSlot5;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // DevKey
    private readonly InputActionMap m_DevKey;
    private IDevKeyActions m_DevKeyActionsCallbackInterface;
    private readonly InputAction m_DevKey_AddItem;
    private readonly InputAction m_DevKey_RemoveItem;
    public struct DevKeyActions
    {
        private @PlayerInput m_Wrapper;
        public DevKeyActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddItem => m_Wrapper.m_DevKey_AddItem;
        public InputAction @RemoveItem => m_Wrapper.m_DevKey_RemoveItem;
        public InputActionMap Get() { return m_Wrapper.m_DevKey; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DevKeyActions set) { return set.Get(); }
        public void SetCallbacks(IDevKeyActions instance)
        {
            if (m_Wrapper.m_DevKeyActionsCallbackInterface != null)
            {
                @AddItem.started -= m_Wrapper.m_DevKeyActionsCallbackInterface.OnAddItem;
                @AddItem.performed -= m_Wrapper.m_DevKeyActionsCallbackInterface.OnAddItem;
                @AddItem.canceled -= m_Wrapper.m_DevKeyActionsCallbackInterface.OnAddItem;
                @RemoveItem.started -= m_Wrapper.m_DevKeyActionsCallbackInterface.OnRemoveItem;
                @RemoveItem.performed -= m_Wrapper.m_DevKeyActionsCallbackInterface.OnRemoveItem;
                @RemoveItem.canceled -= m_Wrapper.m_DevKeyActionsCallbackInterface.OnRemoveItem;
            }
            m_Wrapper.m_DevKeyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AddItem.started += instance.OnAddItem;
                @AddItem.performed += instance.OnAddItem;
                @AddItem.canceled += instance.OnAddItem;
                @RemoveItem.started += instance.OnRemoveItem;
                @RemoveItem.performed += instance.OnRemoveItem;
                @RemoveItem.canceled += instance.OnRemoveItem;
            }
        }
    }
    public DevKeyActions @DevKey => new DevKeyActions(this);
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IAttackActions
    {
        void OnShoot(InputAction.CallbackContext context);
    }
    public interface IMouseActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnInventory(InputAction.CallbackContext context);
        void OnSlot1(InputAction.CallbackContext context);
        void OnSlot2(InputAction.CallbackContext context);
        void OnSlot3(InputAction.CallbackContext context);
        void OnSlot4(InputAction.CallbackContext context);
        void OnSlot5(InputAction.CallbackContext context);
    }
    public interface IDevKeyActions
    {
        void OnAddItem(InputAction.CallbackContext context);
        void OnRemoveItem(InputAction.CallbackContext context);
    }
}
