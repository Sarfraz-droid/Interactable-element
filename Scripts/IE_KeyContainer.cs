using TMPro;
using UnityEngine;

public class IE_KeyContainer : MonoBehaviour
{
    public IE_KeyOverrides keyOverrides;
    public TextMeshProUGUI keyText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyText(PathDetails pathDetails, string text) {
        keyText.text = text;
        keyOverrides.ApplyOverride(pathDetails);
    }
}
