using UnityEngine;

public class IE_FPSInteractionUIContainer : MonoBehaviour
{

    public GameObject onUIGameObject;
    public GameObject inFrontGameObject;

    public GameObject inFrontKeyContainer;
    [SerializeField] private GameObject keyContainerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInteraction() {
        if(IE_InteractionBehaviour.instance.currentInteractionElement.interactionType == InteractionType.InFront) {
            UpdateInFrontInteraction();
        } else {
            UpdateOnUIInteraction();
        }
    }

    public void UpdateOnUIInteraction() {
        onUIGameObject.SetActive(true);
        inFrontGameObject.SetActive(false);

        DeleteKeys(onUIGameObject);
        CreateKeys(IE_InteractionBehaviour.instance.currentInteractionElement.interactionContent, onUIGameObject);
    }

    public void UpdateInFrontInteraction() {
        onUIGameObject.SetActive(false);
        inFrontGameObject.SetActive(true);

        IE_KeyContainer keyContainer = inFrontKeyContainer.GetComponent<IE_KeyContainer>();

        InteractionContent[] interactionContent = IE_InteractionBehaviour.instance.currentInteractionElement.interactionContent;  


        keyContainer.ApplyText(interactionContent[0].GetKeyCode(), interactionContent[0].interactionText);

    }

    public void CreateKeys(InteractionContent[] interactionContent, GameObject obj) {
        foreach(InteractionContent content in interactionContent) {
            GameObject keyContainer = Instantiate(keyContainerPrefab, obj.transform);
            keyContainer.GetComponent<IE_KeyContainer>().ApplyText(content.GetKeyCode(), content.interactionText);
        }
    }

    public void DeleteKeys(GameObject obj) {
        while(obj.transform.childCount > 0) {
            DestroyImmediate(obj.transform.GetChild(0).gameObject);
        }
    }
}
