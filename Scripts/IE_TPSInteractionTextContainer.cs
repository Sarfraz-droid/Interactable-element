using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IE_TPSInteractionTextContainer : MonoBehaviour
{
    public GameObject keyContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInteractionText() {
        IE_InteractionElement interactionElement = IE_InteractionBehaviour.instance.currentInteractionElement;

        Vector3 point = IE_InteractionBehaviour.instance.currentPoint;

        InteractionContent[] interactionContents = interactionElement.interactionContent;

        InteractionContent content = interactionContents[0];

        IE_KeyContainer keyContainerScript = keyContainer.GetComponent<IE_KeyContainer>();

        keyContainerScript.ApplyText(content.GetKeyCode(), content.interactionText);

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(point);

        transform.position = screenPoint;


        // KeyOverride keyOverride = keyOverrides.GetKeyOverride(content.GetKeyCode()?);

        // if(keyOverride != null) {
        //     if(keyOverride.hasText) {
        //         keyOverrides.ApplyOverride(content.GetKeyCode());
        //     } else {
        //         keyOverrides.ApplyOverride(content.GetKeyCode());
        //     }
        // } else {
        //     keyOverrides.ApplyOverride(content.GetKeyCode());
        // }

        // interactionText.text = content.interactionText;
        


    }
}
