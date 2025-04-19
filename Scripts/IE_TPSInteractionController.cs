using UnityEngine;
using UnityEngine.InputSystem;

public class IE_TPSInteractionController : MonoBehaviour
{
    public float interactionRadius = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InteractionWatcher();
    }

    void FixedUpdate()
    {
        TPSInteraction();
    }

    public void TPSInteraction()
    {
        if (!IE_InteractionBehaviour.instance.areInteractionsEnabled)
        {
            IE_InteractionBehaviour.instance.currentInteractionElement = null;
            return;
        }

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactionRadius, transform.forward, interactionRadius, IE_InteractionBehaviour.instance.GetLayerMask());


        bool foundInteractionElement = false;

        float closestDistance = float.MaxValue;
        IE_InteractionElement closestInteractionElement = null;
        Vector3 closestPoint = Vector3.zero;

        foreach (RaycastHit hit in hits)
        {
            IE_InteractionElement interactionElement = hit.collider.GetComponent<IE_InteractionElement>();

            if (interactionElement == null)
            {
                interactionElement = hit.collider.GetComponentInParent<IE_InteractionElement>();
            }

            Debug.Log("Interaction Element: " + interactionElement + " - " + hit.point + " - " + hit.collider);

            Vector3 point = hit.point;

            if (point == Vector3.zero)
            {
                point = hit.collider.ClosestPoint(transform.position);
            }

            if (interactionElement != null && hit.collider != null)
            {
                // IE_InteractionBehaviour.instance.UpdateInteractionContent(interactionElement, point);
                // foundInteractionElement = true;

                float distance = Vector3.Distance(transform.position, point);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractionElement = interactionElement;
                    closestPoint = point;
                    foundInteractionElement = true;
                }
            }

        }

        if (foundInteractionElement)
        {
            IE_InteractionBehaviour.instance.ToggleInteractions(true);
            IE_InteractionBehaviour.instance.UpdateInteractionContent(closestInteractionElement, closestPoint);
        }
        else
        {
            IE_InteractionBehaviour.instance.ToggleInteractions(false);
            IE_InteractionBehaviour.instance.ClearInteractionContent();
        }
    }


    public void InteractionWatcher()
    {
        if (IE_InteractionBehaviour.instance.currentInteractionElement == null)
        {
            IE_InteractionBehaviour.instance.interactionConfig.isLoading = false;
            return;
        }

        bool isInteracting = false;

        IE_InteractionElement interactionElement = IE_InteractionBehaviour.instance.currentInteractionElement;

        InteractionContent[] interactionContents = interactionElement.interactionContent;
        Debug.Log("InputSystem enabled");
        foreach (InteractionContent content in interactionContents)
        {
            InputAction inputAction = InputSystemUtils.GetInputAction(content.inputActionReference, IE_InteractionBehaviour.instance.inputActionAsset);

            if (inputAction == null)
            {
                Debug.LogError("InputAction not found: " + content.inputActionReference);
                continue;
            }

            Debug.Log("InputActionEvent: " + inputAction.name + " | " + inputAction.IsPressed() + " | " + inputAction.triggered + " | ");

            if (content.interactionDelay > 0)
            {
                if (inputAction.IsPressed())
                {
                    isInteracting = true;
                    ResolveInteraction(content, interactionElement);
                }
            }
            else
            {
                if (inputAction.triggered)
                {
                    isInteracting = true;
                    ResolveInteraction(content, interactionElement);
                }
            }
        }
        if (!isInteracting)
        {
            IE_InteractionBehaviour.instance.interactionConfig = new InteractionConfig();
        }
    }

    void ResolveInteraction(InteractionContent content, IE_InteractionElement interactionElement)
    {
        InteractionConfig interactionConfig = IE_InteractionBehaviour.instance.interactionConfig;

        Debug.Log("Resolving interaction");
        if (interactionConfig.element != interactionElement)
        {
            interactionConfig = new InteractionConfig
            {
                element = interactionElement,
                content = content,
                timeToInteract = Time.time,
                isLoading = true
            };
        }
        else
        {
            if (interactionConfig.content != content)
            {
                interactionConfig.timeToInteract = Time.time;
            }
        }
        float timeSinceInteraction = Time.time - interactionConfig.timeToInteract;

        if (timeSinceInteraction >= content.interactionDelay)
        {
            interactionConfig.isLoading = false;
            interactionConfig.timeToInteract = Time.time;
            interactionConfig.hasInteracted = true;
            IE_InteractionBehaviour.instance.interactionTextUILayer.UpdateInteractionText();
            content.onInteract.Invoke(content);
        }

        IE_InteractionBehaviour.instance.interactionConfig = interactionConfig;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
