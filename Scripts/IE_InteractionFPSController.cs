using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionFPSController : MonoBehaviour
{
    public float interactionDistance = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastInteraction();
    }

    void Update()
    {
        InteractionWatcher();
    }


    // Check if camera is looking at an object with the interaction Layer
    public void RaycastInteraction()
    {
        if (!IE_InteractionBehaviour.instance.areInteractionsEnabled) {
            IE_InteractionBehaviour.instance.currentInteractionElement = null;
            return;
        }

        bool foundInteractionElement = false;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance))
        {
            IE_InteractionBehaviour.instance.debugPoint = hit.point;

            GameObject obj = hit.collider.gameObject;

            IE_InteractionElement interactionElement = obj.GetComponent<IE_InteractionElement>();

            if(interactionElement == null) {
                interactionElement = obj.GetComponentInParent<IE_InteractionElement>();
            }

            if(interactionElement != null) {
                foundInteractionElement = true;
            }

            if(IE_InteractionBehaviour.instance.currentInteractionElement != interactionElement) {
                Debug.Log("Updating interaction text"); 
                IE_InteractionBehaviour.instance.ToggleInteractions(false);
                IE_InteractionBehaviour.instance.currentInteractionElement = interactionElement;
                IE_InteractionBehaviour.instance.interactionTextUILayer.UpdateInteractionText();
                IE_InteractionBehaviour.instance.ToggleInteractions(true);
            }
        }

        if(!foundInteractionElement && IE_InteractionBehaviour.instance.currentInteractionElement != null) {
            IE_InteractionBehaviour.instance.currentInteractionElement = null;
            IE_InteractionBehaviour.instance.interactionTextUILayer.UpdateInteractionText();
            IE_InteractionBehaviour.instance.ToggleInteractions(false);
        }
    }


    public void InteractionWatcher() {
        if(IE_InteractionBehaviour.instance.currentInteractionElement == null) {
            IE_InteractionBehaviour.instance.interactionConfig = new InteractionConfig();
            return;
        }

        bool isInteracting = false;

        IE_InteractionElement interactionElement = IE_InteractionBehaviour.instance.currentInteractionElement;

        InteractionContent[] interactionContents = interactionElement.interactionContent;
        foreach(InteractionContent content in interactionContents) {
            InputAction inputAction = InputSystemUtils.GetInputAction(content.inputActionReference, IE_InteractionBehaviour.instance.inputActionAsset);

            if(inputAction == null) {
                Debug.LogError("InputAction not found: " + content.inputActionReference);
                continue;
            }

            Debug.Log("InputActionEvent: " + inputAction.name + " | " + inputAction.IsPressed() + " | " + inputAction.triggered + " | " );

            if(content.interactionDelay > 0) {
                if(inputAction.IsPressed()) {
                    isInteracting = true;
                    ResolveInteraction(content, interactionElement);
                }
            } else {
                if(inputAction.triggered) {
                    isInteracting = true;
                    ResolveInteraction(content, interactionElement);
                }
            }
        }
        if(!isInteracting) {
            IE_InteractionBehaviour.instance.interactionConfig = new InteractionConfig();
        }
    }

    void ResolveInteraction(InteractionContent content, IE_InteractionElement interactionElement) {
        InteractionConfig interactionConfig = IE_InteractionBehaviour.instance.interactionConfig;

        Debug.Log("Resolving interaction");
        if(interactionConfig.element != interactionElement){ 
            interactionConfig = new InteractionConfig {
                element = interactionElement,
                content = content,
                timeToInteract = Time.time,
                isLoading = true
            };
        } else {
            if(interactionConfig.content != content) {
                interactionConfig.timeToInteract = Time.time;
            }
        }
        float timeSinceInteraction = Time.time - interactionConfig.timeToInteract;

        Debug.Log("Time since interaction: " + timeSinceInteraction);

        if(timeSinceInteraction >= content.interactionDelay) {
            interactionConfig.isLoading = false;
            interactionConfig.timeToInteract = Time.time;
            interactionConfig.hasInteracted = true;
            Debug.Log("Interacting with " + interactionElement.name);
            IE_InteractionBehaviour.instance.interactionTextUILayer.UpdateInteractionText();
            content.onInteract.Invoke(content);
        }

        IE_InteractionBehaviour.instance.interactionConfig = interactionConfig;
    }
}
