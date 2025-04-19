using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(KeyOverride))]
public class KeyOverrideDrawer : PropertyDrawer {
    public VisualTreeAsset VisualTree;

    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        VisualElement root = new VisualElement();
        VisualTree.CloneTree(root);

        DropdownField dropdown = root.Q<DropdownField>("KeyCodeDropdown");


        SerializedProperty keyCodeProperty = property.FindPropertyRelative("keyCode");
        SerializedProperty hasTextProperty = property.FindPropertyRelative("hasText");
        SerializedProperty textInputProperty = property.FindPropertyRelative("keyText");

        dropdown.choices = new List<string> { };

        foreach(string binding in BindingsMap.GetAllBindings()) {
            dropdown.choices.Add(binding);
        }

        if(keyCodeProperty.stringValue != null) {
            dropdown.value = keyCodeProperty.stringValue;
        }

        dropdown.RegisterValueChangedCallback(evt => OnDropdownValueChanged(evt, ref keyCodeProperty));

        PropertyField propertyField = root.Q<PropertyField>("HasText");
        PropertyField textInputField = root.Q<PropertyField>("KeyText");

        propertyField.RegisterCallback<ChangeEvent<bool>>(evt => OnHasTextChanged(evt, ref hasTextProperty, textInputField));

        ShowTextInput(hasTextProperty.boolValue, textInputField);

        return root;
    }
    private void OnDropdownValueChanged(ChangeEvent<string> evt, ref SerializedProperty property) {
        property.stringValue = evt.newValue;
        property.serializedObject.ApplyModifiedProperties();
    }

    private void ShowTextInput(bool show, PropertyField propertyField) {
        if(show) {
            propertyField.style.display = DisplayStyle.Flex;
        } else {
            propertyField.style.display = DisplayStyle.None;
        }
    }

    private void OnHasTextChanged(ChangeEvent<bool> evt, ref SerializedProperty property, PropertyField propertyField) {
        property.boolValue = evt.newValue;
        property.serializedObject.ApplyModifiedProperties();

        ShowTextInput(evt.newValue, propertyField);
    }
}