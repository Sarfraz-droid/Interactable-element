using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public enum DeviceTypes {
    Mouse,
    Keyboard,
    XInputController
}

[Serializable]
public class IEManagerConfig {  
    public InputActionAsset inputActionAsset;

    public static IEManagerConfig instance;

    private IEManagerConfig() {
        instance = this;
    }

    public static IEManagerConfig GetInstance() {
        if(instance == null) {
            instance = new IEManagerConfig();
        }
        return instance;
    }

}



[Serializable]
public class InteractionContent {
    
    
    public string inputActionReference;
    public string interactionText;

    public int interactionDelay = 0;


    public UnityEvent<InteractionContent> onInteract;

    public string GenerateInteractionText() {
        return interactionText;
    }
    
    public PathDetails GetKeyCode() {

        InputType inputType = IE_InteractionBehaviour.instance.inputElementType;


        InputAction inputAction = InputSystemUtils.GetInputAction(inputActionReference);
        InputControl control = inputAction.activeControl;

            InputBinding[] inputBindings = inputAction.bindings.ToArray();

            Debug.Log("GetKeyCode: " + inputBindings.Length + " | " + inputType);

            if(inputType == InputType.Gamepad) {
                int gamepadIndex = inputAction.GetBindingIndex(InputBinding.MaskByGroup("Gamepad"));

                string groups = "";
                foreach(InputBinding binding in inputBindings) {
                    groups += binding.groups + " | ";
                }

                Debug.Log("GetKeyCode:Groups: " + groups);

                Debug.Log("GetKeyCode: " + inputBindings.Length + " | " + inputType + " | " + gamepadIndex + " | " + groups);
                if(gamepadIndex != -1) {
                    InputBinding binding = inputBindings[gamepadIndex];

                    PathDetails pathDetails = InputSystemUtils.GetPathDetails(binding);
                    Debug.Log("GetKeyCode:PathDetails: " + pathDetails + " | " + binding.action);

                    return pathDetails;
                }
            }

            if(inputType == InputType.KeyboardMouse) {
                int keyboardIndex = inputAction.GetBindingIndex(InputBinding.MaskByGroup("KeyboardMouse"));

                Debug.Log("GetKeyCode: " + inputBindings.Length + " | " + inputType + " | " + keyboardIndex);

                if(keyboardIndex != -1) {
                    InputBinding binding = inputBindings[keyboardIndex];

                    PathDetails pathDetails = InputSystemUtils.GetPathDetails(binding);

                    Debug.Log("GetKeyCode:PathDetails: " + pathDetails);

                    return pathDetails;
                }
            }

            InputBinding binding1 = inputBindings[0];
            PathDetails pDetails = InputSystemUtils.GetPathDetails(binding1);

            return pDetails;
    }
}

[Serializable]
public class InteractionConfig {
    public IE_InteractionElement element;
    public InteractionContent content;
    public float timeToInteract;

    public bool isLoading = false;

    public bool hasInteracted = false;
}

[Serializable]
public enum InteractionUserType {
    FPS,
    TPS
}

[Serializable]
public enum InputType {
    KeyboardMouse,
    Gamepad
}


[Serializable]
public class KeyOverride {
    public string keyCode;

    public Sprite sprite;
    public bool hasText;
    public string keyText;
}

[Serializable]
public class InterfactionElementConfig {
    public bool disableOnInteract = false;
}
