//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Movements"",
            ""id"": ""7d37ac24-2ecf-4082-8432-2e7788f355c8"",
            ""actions"": [
                {
                    ""name"": ""LeftRight"",
                    ""type"": ""Value"",
                    ""id"": ""62bca1dc-863d-49e9-b2fe-d4426fc403c7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Accl"",
                    ""type"": ""Value"",
                    ""id"": ""0339a8cd-9346-4ef9-bb24-a0c41658a161"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""LR"",
                    ""id"": ""2a6226e9-f089-482c-b677-7a474ef50581"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftRight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0197caa9-3c1a-48ce-8dc5-20edb30a49a7"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c9ec11a0-1fec-4bc7-9f03-b48a47068338"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""76ba2447-a8d1-464b-a83d-b585cd3af872"",
                    ""path"": ""<Accelerometer>/acceleration/y"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone(min=1.2,max=7)"",
                    ""groups"": """",
                    ""action"": ""LeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Accl/Decl"",
                    ""id"": ""75e9c7f2-ece9-4d82-9b0a-384a475736bc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accl"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""86009f43-5e88-4045-b6ca-102739d7acb2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""43802841-9327-461b-8652-37e7c3169e0f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movements
        m_Movements = asset.FindActionMap("Movements", throwIfNotFound: true);
        m_Movements_LeftRight = m_Movements.FindAction("LeftRight", throwIfNotFound: true);
        m_Movements_Accl = m_Movements.FindAction("Accl", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Movements
    private readonly InputActionMap m_Movements;
    private List<IMovementsActions> m_MovementsActionsCallbackInterfaces = new List<IMovementsActions>();
    private readonly InputAction m_Movements_LeftRight;
    private readonly InputAction m_Movements_Accl;
    public struct MovementsActions
    {
        private @PlayerInputActions m_Wrapper;
        public MovementsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftRight => m_Wrapper.m_Movements_LeftRight;
        public InputAction @Accl => m_Wrapper.m_Movements_Accl;
        public InputActionMap Get() { return m_Wrapper.m_Movements; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementsActions set) { return set.Get(); }
        public void AddCallbacks(IMovementsActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementsActionsCallbackInterfaces.Add(instance);
            @LeftRight.started += instance.OnLeftRight;
            @LeftRight.performed += instance.OnLeftRight;
            @LeftRight.canceled += instance.OnLeftRight;
            @Accl.started += instance.OnAccl;
            @Accl.performed += instance.OnAccl;
            @Accl.canceled += instance.OnAccl;
        }

        private void UnregisterCallbacks(IMovementsActions instance)
        {
            @LeftRight.started -= instance.OnLeftRight;
            @LeftRight.performed -= instance.OnLeftRight;
            @LeftRight.canceled -= instance.OnLeftRight;
            @Accl.started -= instance.OnAccl;
            @Accl.performed -= instance.OnAccl;
            @Accl.canceled -= instance.OnAccl;
        }

        public void RemoveCallbacks(IMovementsActions instance)
        {
            if (m_Wrapper.m_MovementsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementsActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementsActions @Movements => new MovementsActions(this);
    public interface IMovementsActions
    {
        void OnLeftRight(InputAction.CallbackContext context);
        void OnAccl(InputAction.CallbackContext context);
    }
}
