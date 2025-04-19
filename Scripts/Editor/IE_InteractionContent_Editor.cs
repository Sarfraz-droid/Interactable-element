using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.InputSystem;

[CustomPropertyDrawer(typeof(InteractionContent))]
public class IE_InteractionContent_Editor : PropertyDrawer
{
    public VisualTreeAsset VisualTree;
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        VisualTree.CloneTree(root);

        DropdownField dropdown = root.Q<DropdownField>("InteractionActionReference");
        PropertyField keyCodeField = root.Q<PropertyField>("keyCode");

        GameObject[] eventHandlers = GameObject.FindGameObjectsWithTag("IE_EventHandler");
        
        if(eventHandlers.Length == 0) {
            Debug.LogError("No IE_EventHandler found");

            Label label = new Label("No Interaction Event Handlers found");
            label.style.color = Color.red;
            root.Clear();

            root.Add(label);

            return root;
        }

        GameObject eventHandler = eventHandlers[0];

        IE_InteractionBehaviour eventHandlerScript = eventHandler.GetComponent<IE_InteractionBehaviour>();

        if(eventHandlerScript == null) {
            Debug.LogError("IE_EventHandler script not found");

            Label label = new Label("IE_EventHandler script not found");        
            label.style.color = Color.red;

            root.Clear();

            root.Add(label);

            return root;
        }


        dropdown.choices = new List<string> { };



        List<InputAction> actions = InputSystemUtils.GetActions(eventHandlerScript.inputActionAsset);

        foreach(InputAction action in actions) {
            dropdown.choices.Add(action.name);
        }

        SerializedProperty inputActionReferenceProperty = property.FindPropertyRelative("inputActionReference");
        
        bool isInputActionChoicePresent = false;

        if(dropdown.choices.Contains(inputActionReferenceProperty.stringValue)) {
            isInputActionChoicePresent = true;
        }

        if(isInputActionChoicePresent) {
            dropdown.value = inputActionReferenceProperty.stringValue;
        } else {
            dropdown.value = "< No Input Action Choice Present >";
        }

        Debug.Log(inputActionReferenceProperty.stringValue);
        

        dropdown.RegisterCallback<ChangeEvent<string>>(evt => OnDropdownChange(evt, ref inputActionReferenceProperty));

        return root;
    }

    private void OnDropdownChange(ChangeEvent<string> evt, ref SerializedProperty property) {
        property.stringValue = evt.newValue;

        Debug.Log(evt.newValue + " | " + property.stringValue);

        GameObject[] eventHandlers = GameObject.FindGameObjectsWithTag("IE_EventHandler");

        if(eventHandlers.Length == 0) {
            Debug.LogError("No IE_EventHandler found");
            return;
        }

        GameObject eventHandler = eventHandlers[0];
        InputAction inputAction = InputSystemUtils.GetInputAction(evt.newValue, eventHandler.GetComponent<IE_InteractionBehaviour>().inputActionAsset);

        if(inputAction == null) {
            Debug.LogError("InputAction not found: " + evt.newValue);
            return;
        }

        string keyCode = InputSystemUtils.GetKeyCode(inputAction);

        Debug.Log("OnDropdownChange: " + keyCode);

        property.serializedObject.ApplyModifiedProperties();
    }   
}
