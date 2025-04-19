using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IE_KeyOverrides : MonoBehaviour
{
    Dictionary<string, KeyOverride> keyOverrideDict;

    KeysInfoObject keysInfoObject;

    public TextMeshProUGUI text;

    Image image;

    bool isInitialized = false;

    void Start()
    {
        Initialize();
    }

    public void Initialize() {
        if(isInitialized) {
            return;
        }

        keysInfoObject = IE_InteractionBehaviour.instance.keysInfoObject;

        keyOverrideDict = new Dictionary<string, KeyOverride>();
        foreach (KeyOverride keyOverride in keysInfoObject.keyOverrides) {
            keyOverrideDict[keyOverride.keyCode.ToString()] = keyOverride;
        }
        
        image = GetComponent<Image>();

        isInitialized = true;
    }

    public int GetRelativeHeight(Sprite sprite) {
        int width = keysInfoObject.width;

        float spriteWidth = sprite.rect.size.x;
        float spriteHeight = sprite.rect.size.y;

        float relativeHeight = spriteHeight / spriteWidth * width;

        return (int)relativeHeight;
    }



    public KeyOverride GetKeyOverride(string action) {
        if(keyOverrideDict.ContainsKey(action)) {
            return keyOverrideDict[action];
        }
        return null;
    }


    public void ApplyOverride(PathDetails? pathDetails) {
        if(!isInitialized) {
            Initialize();
        }

        int width = keysInfoObject.width;
        
        if(pathDetails == null) {
            image.sprite = keysInfoObject.defaultSprite;
            Sprite sprite = keysInfoObject.defaultSprite;
            
            int height = GetRelativeHeight(sprite);

            image.rectTransform.sizeDelta = new Vector2(width, height);
            return;
        }

        KeyOverride keyOverride = GetKeyOverride(pathDetails.action);
        if(keyOverride == null) {
            image.sprite = keysInfoObject.defaultSprite;
            Sprite sprite = keysInfoObject.defaultSprite;

            int height = GetRelativeHeight(sprite);

            image.rectTransform.sizeDelta = new Vector2(width, height);
        } else {
            image.sprite = keyOverride.sprite;
            Sprite sprite = keyOverride.sprite;

            int height = GetRelativeHeight(sprite);

            image.rectTransform.sizeDelta = new Vector2(width, height);
        }

        if(keyOverride != null) {
            if(keyOverride.hasText) {
                text.text = keyOverride.keyText;
            } else {
                text.text = "";
            }
        } else {
            text.text = pathDetails.action.ToUpper();
        }
    }
}