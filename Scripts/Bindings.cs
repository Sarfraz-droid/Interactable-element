using System.Collections.Generic;

public enum ControllerBindingList {
    buttonSouth,
    buttonEast,
    buttonWest,
    buttonNorth,
    rightStickPress,
    leftStickPress,
    leftTrigger,
    rightTrigger,
    dpadUp,
    dpadDown,
    dpadLeft,
    dpadRight,
    select,
    start,
}

public enum KeyboardBindingList {
    leftButton,
    rightButton,
}

public class BindingsMap {
    public static Dictionary<string, string> controllerBindings = new Dictionary<string, string> {
        { "buttonSouth", "A" },
        { "buttonEast", "B" },
        { "buttonWest", "X" },
        { "buttonNorth", "Y" },
        { "rightStickPress", "L3" },
        { "leftStickPress", "R3" },
        { "dpad/down", "DPAD_DOWN" },
        { "dpad/up", "DPAD_UP" },
        { "dpad/left", "DPAD_LEFT" },
        { "dpad/right", "DPAD_RIGHT" },
        { "start", "START" },
        { "select", "SELECT" },
        { "leftTrigger", "LEFT_TRIGGER" },
        { "rightTrigger", "RIGHT_TRIGGER" },
        { "leftShoulder", "LEFT_SHOULDER" },
        { "rightShoulder", "RIGHT_SHOULDER" },
    };

    public static Dictionary<string, string> keyboardBindings = new Dictionary<string, string> {
        { KeyboardBindingList.leftButton.ToString(), "Mouse0" },
        { KeyboardBindingList.rightButton.ToString(), "Mouse1" },
    };

    public static string GetBinding(string action) {
        InputType inputType = IE_InteractionBehaviour.instance.inputElementType;

        if(inputType == InputType.Gamepad) {
            if(controllerBindings.ContainsKey(action)) {
                return controllerBindings[action];
            }
        } else if(inputType == InputType.KeyboardMouse) {
            if(keyboardBindings.ContainsKey(action)) {
                return keyboardBindings[action];
            }
        }
        return action;
    }

    public static string[] GetAllBindings() {
        List<string> bindings = new List<string>();

        foreach(var binding in controllerBindings) {
            bindings.Add(binding.Value);
        }

        foreach(var binding in keyboardBindings) {
            bindings.Add(binding.Value);
        }

        return bindings.ToArray();
    }
}