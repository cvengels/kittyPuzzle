// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerInput.inputactions'

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
            ""name"": ""Player"",
            ""id"": ""a00e2e36-2a1b-441f-a88d-767bfbf84e1d"",
            ""actions"": [
                {
                    ""name"": ""GridMovement"",
                    ""type"": ""Button"",
                    ""id"": ""293aacf2-2aeb-4bd4-9b71-02b259b4a1e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Reset Level"",
                    ""type"": ""Button"",
                    ""id"": ""6e30b712-8857-48c2-8d5e-ceb854ea7509"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""35cdd161-931f-4915-97c8-c00fa136ad80"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GridMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""75a4a869-e59c-4e44-ad56-2e04afc86138"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9d16becb-4e75-43d1-bf2e-df920e644e22"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dfeec21f-87db-44ea-a094-fbfc514302c5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2caf8beb-eb85-44d1-ab4f-f1a0a4e234c8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""cda53e0d-f9a2-4940-85f7-663a69b920b6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""dda5549d-8891-4c34-a5f1-0ed6d45b39d6"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6daa4ab6-37ab-469e-b444-a473742b1101"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ba163f58-c18e-4bfd-97c2-87f3be44bd5f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e0f8fd06-f730-4ce9-9de6-b7d3ef6c7902"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""GridMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""27b15d70-30c5-40c7-a438-cb31d35b83db"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Reset Level"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard Control"",
            ""bindingGroup"": ""Keyboard Control"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_GridMovement = m_Player.FindAction("GridMovement", throwIfNotFound: true);
        m_Player_ResetLevel = m_Player.FindAction("Reset Level", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_GridMovement;
    private readonly InputAction m_Player_ResetLevel;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @GridMovement => m_Wrapper.m_Player_GridMovement;
        public InputAction @ResetLevel => m_Wrapper.m_Player_ResetLevel;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @GridMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGridMovement;
                @GridMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGridMovement;
                @GridMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGridMovement;
                @ResetLevel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetLevel;
                @ResetLevel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetLevel;
                @ResetLevel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetLevel;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @GridMovement.started += instance.OnGridMovement;
                @GridMovement.performed += instance.OnGridMovement;
                @GridMovement.canceled += instance.OnGridMovement;
                @ResetLevel.started += instance.OnResetLevel;
                @ResetLevel.performed += instance.OnResetLevel;
                @ResetLevel.canceled += instance.OnResetLevel;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardControlSchemeIndex = -1;
    public InputControlScheme KeyboardControlScheme
    {
        get
        {
            if (m_KeyboardControlSchemeIndex == -1) m_KeyboardControlSchemeIndex = asset.FindControlSchemeIndex("Keyboard Control");
            return asset.controlSchemes[m_KeyboardControlSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnGridMovement(InputAction.CallbackContext context);
        void OnResetLevel(InputAction.CallbackContext context);
    }
}
