using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IE_InteractionTextUILayer : MonoBehaviour
{
    public InteractionUserType interactionUserType;

    [SerializeField] private IE_FPSInteractionUIContainer fpsInteractionUIContainer;
    [SerializeField] private IE_TPSInteractionTextContainer tpsInteractionTextContainer;
    [SerializeField] private Image loadingTexture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(interactionUserType == InteractionUserType.FPS) {
            fpsInteractionUIContainer.gameObject.SetActive(false);
        } else {
            tpsInteractionTextContainer.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        UpdateLoadingTexture();
    }

    public void UpdateLoadingTexture() {
        InteractionConfig interactionConfig = IE_InteractionBehaviour.instance.interactionConfig;

        if(interactionConfig.isLoading) {
            loadingTexture.gameObject.SetActive(true);

            float timeSinceInteraction = Time.time - interactionConfig.timeToInteract;

            loadingTexture.fillAmount = timeSinceInteraction / interactionConfig.content.interactionDelay;
        } else {
            loadingTexture.gameObject.SetActive(false);
        }
    }

    public void UpdateInteractionText() {
        Debug.Log("Updating interaction text: " + ShouldShowInteractionText());
        if(ShouldShowInteractionText()) {
            Debug.Log("Showing interaction text");
            if(interactionUserType == InteractionUserType.FPS) {
                HandleInteraction();
            } else {
                HandleTPSInteraction();
            }
        } else {
            if(interactionUserType == InteractionUserType.FPS) {
                fpsInteractionUIContainer.gameObject.SetActive(false);
            } else {
                tpsInteractionTextContainer.gameObject.SetActive(false);
            }
        }

    }

    public bool ShouldShowInteractionText() {
        if(IE_InteractionBehaviour.instance.currentInteractionElement == null) {
            return false;
        }

        IE_InteractionElement interactionElement = IE_InteractionBehaviour.instance.currentInteractionElement;
        
        if(!interactionElement.isInteractable) {
            return false;
        }

        InterfactionElementConfig interactionElementConfig = IE_InteractionBehaviour.instance.interactionElementConfig;
        InteractionConfig interactionConfig = IE_InteractionBehaviour.instance.interactionConfig;

        if(interactionElement.isOverride) {
            if(interactionElement.overrideConfig.disableOnInteract && interactionConfig.hasInteracted) {
                return false;
            }

            return true;
        } else {
            Debug.Log("Checking interaction text for tps" + interactionConfig.hasInteracted + " " + interactionElementConfig.disableOnInteract);
            if(interactionConfig.hasInteracted && interactionElementConfig.disableOnInteract) {
                return false;
            }
            return true;
        }
    }

    public void HandleInteraction() {
        fpsInteractionUIContainer.gameObject.SetActive(true);
        fpsInteractionUIContainer.UpdateInteraction();
    }

    public void HandleTPSInteraction() {

        Debug.Log("Updating tps interaction text");
        tpsInteractionTextContainer.gameObject.SetActive(true);

        tpsInteractionTextContainer.UpdateInteractionText();

    }
}
