using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PathDetails {
    public string device;
    public string action;

    public override string ToString() {
        return device + " | " + action;
    }
}

public static class InputSystemUtils {
    public static List<InputAction> GetActions(InputActionAsset inputActionAsset) {
        List<InputAction> actions = new List<InputAction>();
        

        foreach(InputActionMap actionMap in inputActionAsset.actionMaps) {
            actions.AddRange(actionMap.actions);
        }

        return actions;
    }

    public static string GetKeyCode(InputAction inputAction) {
        InputBinding inputBinding = inputAction.bindings[0];
        Debug.Log(inputBinding.path + " | " + inputBinding.groups + " | " + inputAction.name + " | ");

        foreach(InputBinding binding in inputAction.bindings) {
            Debug.Log(binding.path + " | " + binding.groups);
        }

        return inputBinding.path + " | " + inputBinding.groups;
    }

    public static InputAction GetInputAction(string inputActionName) {
        IE_InteractionBehaviour eventHandler = IE_InteractionBehaviour.instance;

        if(eventHandler == null) {
            Debug.LogError("IE_InteractionBehaviour not found");
            return null;
        }

        InputAction inputAction = eventHandler.inputActionAsset.FindAction(inputActionName);

        return inputAction;
    }

    public static InputAction GetInputAction(string inputActionName, InputActionAsset inputActionAsset) {

        InputAction inputAction = inputActionAsset.FindAction(inputActionName);

        return inputAction;
    }

    public static PathDetails GetPathDetails(InputAction inputAction) {
        PathDetails pathDetails = new PathDetails();

        string devicePath = IE_InteractionBehaviour.instance.devicePath;

        Regex regex = new("<([^>]+)>/(.+)");

        InputBinding[] inputBindings = inputAction.bindings.ToArray();

        foreach(InputBinding inputBinding in inputBindings) {
            PathDetails tempPathDetails = new PathDetails();
            Match match = regex.Match(inputBinding.path);
            if(match.Success) {
                tempPathDetails.device = match.Groups[1].Value;
                tempPathDetails.action = match.Groups[2].Value;
            }

            if(tempPathDetails.device.Contains(devicePath)) {
                pathDetails = tempPathDetails;
                break;
            }
        }
        return pathDetails;
    }

    public static PathDetails GetPathDetails(InputBinding inputBinding) {
        PathDetails pathDetails = new PathDetails();

        Regex regex = new("<([^>]+)>/(.+)");

        Match match = regex.Match(inputBinding.path);


        if(match.Success) {
            pathDetails.device = match.Groups[1].Value;
            pathDetails.action = BindingsMap.GetBinding(match.Groups[2].Value);
        }
        return pathDetails;
    }
    

    public static PathDetails GetPathDetails(string path) {
        PathDetails pathDetails = new PathDetails();

        Regex regex = new("<([^>]+)>/(.+)");

        Match match = regex.Match(path);

        if(match.Success) {
            pathDetails.device = match.Groups[1].Value;
            pathDetails.action = match.Groups[2].Value;
        }

        return pathDetails;
    }



}
