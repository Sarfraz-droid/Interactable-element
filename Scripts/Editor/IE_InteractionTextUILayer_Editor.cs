using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(IE_InteractionTextUILayer))]
public class IE_InteractionTextUILayer_Editor : Editor
{

    public VisualTreeAsset VisualTree;

    public PropertyField _interactionUserType;

    private VisualElement TPSVariables, FPSVariables;

    public SerializedProperty _interactionPropertyValue;

    void OnEnable()
    {
        _interactionPropertyValue = serializedObject.FindProperty("interactionUserType");
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        VisualTree.CloneTree(root);

        TPSVariables = root.Q<VisualElement>("TPSVariables");
        FPSVariables = root.Q<VisualElement>("FPSVariables");

        _interactionUserType = root.Q<PropertyField>("InteractionUserType");
        Debug.Log("InteractionUserType: " + _interactionUserType + " | " + _interactionUserType.bindingPath);

        _interactionUserType.RegisterValueChangeCallback(OnInteractionUserTypeChange);

        return root;
    }


    // Event Handlers
    private void OnInteractionUserTypeChange(SerializedPropertyChangeEvent evt) {
        Debug.Log("InteractionUserType changed to: " + evt.changedProperty.enumValueIndex);
        if(evt.changedProperty.enumValueIndex == (int)InteractionUserType.TPS) {
            TPSVariables.visible = true;
            FPSVariables.visible = false;
            TPSVariables.style.display = DisplayStyle.Flex;
            FPSVariables.style.display = DisplayStyle.None;
        } else {
            TPSVariables.visible = false;
            FPSVariables.visible = true;
            TPSVariables.style.display = DisplayStyle.None;
            FPSVariables.style.display = DisplayStyle.Flex;
        }
    }
}
