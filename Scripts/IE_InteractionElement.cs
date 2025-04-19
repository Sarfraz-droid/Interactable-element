using UnityEngine;
using UnityEngine.InputSystem;

public class IE_InteractionElement : MonoBehaviour
{
    public bool isInteractable = true;
    public InteractionType interactionType = InteractionType.OnUI;
    public InteractionContent[] interactionContent;

    public bool isOverride = false;

    public InterfactionElementConfig overrideConfig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
