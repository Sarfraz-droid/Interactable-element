using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class IE_InteractionBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    public InputActionAsset inputActionAsset;
    public KeysInfoObject keysInfoObject;


    [Header("Flags")]
    public bool areInteractionsEnabled = true;
    [SerializeField] private bool showDebugRay = false;
    public InterfactionElementConfig interactionElementConfig;
    
    [HideInInspector]
    public string devicePath;

    [HideInInspector]
    public IE_InteractionElement currentInteractionElement;

    [HideInInspector]
    public Vector3 currentPoint;

    [Header("UI")]
    public IE_InteractionTextUILayer interactionTextUILayer;

    // Instance
    public static IE_InteractionBehaviour instance;

    // public Queue<InputControl> inputControls = new Queue<InputControl>();
    public InputType inputElementType;    
    public PathDetails pathDetails;

    // Debug
    [HideInInspector]
    public Vector3 debugPoint;
    
    [HideInInspector]
    public InteractionConfig interactionConfig;  

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        interactionConfig = new InteractionConfig();

        instance = this;
        InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
    }


    private void OnAnyButtonPress(InputControl control)
    {
        InputDevice device = control.device;
        if(device is Gamepad) {
            inputElementType = InputType.Gamepad;
        }
        if(device is Keyboard || device is Mouse) {
            inputElementType = InputType.KeyboardMouse;
        }

        Debug.Log("OnAnyButtonPress: " + control.path);
    }


    private void OnDeviceChange(InputDevice device, InputDeviceChange change) {
        switch(change) {
            case InputDeviceChange.Added:
            case InputDeviceChange.Reconnected:
                if(device is Gamepad) {
                    inputElementType = InputType.Gamepad;
                }
                if(device is Keyboard || device is Mouse) {
                    inputElementType = InputType.KeyboardMouse;
                }
                break;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public LayerMask GetLayerMask() {
        return interactionLayer;
    }

    public void UpdateInteractionContent(IE_InteractionElement interactionContent, Vector3 point) {
        if(currentInteractionElement == null || currentInteractionElement.gameObject.GetInstanceID() != interactionContent.gameObject.GetInstanceID()) {
            currentInteractionElement = interactionContent;
            currentPoint = point;
            Debug.Log("Updating interaction content: " + interactionContent.gameObject.name + " - " + currentInteractionElement.gameObject.name);
            interactionTextUILayer.UpdateInteractionText();
        }
    }

    public void ClearInteractionContent() {
        // ToggleInteractions(false);
        currentInteractionElement = null;
        currentPoint = Vector3.zero;
        interactionTextUILayer.UpdateInteractionText();
    }
    
    public void ToggleInteractions(bool enable) {
        if(currentInteractionElement == null) {
            return;
        }

        Debug.Log("Toggling interactions: " + enable);

        foreach(InteractionContent content in currentInteractionElement.interactionContent) {
            InputAction inputAction = InputSystemUtils.GetInputAction(content.inputActionReference, inputActionAsset);
            if(enable) {
                inputAction.Enable();
            } else {
                inputAction.Disable();
            }
        }
    }
    
    

    public void UpdateInteractionText() {
        interactionTextUILayer.UpdateInteractionText();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(showDebugRay && debugPoint != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            float distance = Vector3.Distance(Camera.main.transform.position, debugPoint);
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance);
            Gizmos.DrawSphere(debugPoint, 0.1f);
        }
    }
}
