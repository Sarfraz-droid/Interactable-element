using UnityEngine;

public class IE_InteractiveCubeHandler : MonoBehaviour
{
    [SerializeField] private GameObject cube;

    public Material defaultMaterial;
    public Material highlightMaterial;

    public bool isHighlighted = false;

    public GameObject ePress;
    public GameObject mouse0Press;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHighlight() {
        isHighlighted = !isHighlighted;

        if(isHighlighted) {
            cube.GetComponent<MeshRenderer>().material = highlightMaterial;
        } else {
            cube.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

    public void ToggleEPress() {
        if(!ePress.activeSelf) {
            mouse0Press.SetActive(false);
        }

        ePress.SetActive(!ePress.activeSelf);
    }

    public void ToggleMouse0Press() {
        if(!mouse0Press.activeSelf) {
            ePress.SetActive(false);
        }
        mouse0Press.SetActive(!mouse0Press.activeSelf);
    }
}
