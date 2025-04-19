using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnalogInputInteractionUtils : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LogAnalogInput(InteractionContent interactionContent) {
        InputAction inputAction = InputSystemUtils.GetInputAction(interactionContent.inputActionReference);
        if(inputAction != null) {
            if(textMeshPro != null) {
                textMeshPro.text = inputAction.ReadValue<float>().ToString("F2");
            }
            Debug.Log("Analog input: " + inputAction.ReadValue<float>());        
        }
    }
}
