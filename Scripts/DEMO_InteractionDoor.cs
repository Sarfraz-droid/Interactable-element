using System.Collections;
using UnityEngine;

public class DEMO_InteractionDoor : MonoBehaviour
{
    public GameObject pivot;

    bool isOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDoor() {
        isOpen = !isOpen;

        StartCoroutine(isOpen ? OpenDoor() : CloseDoor());
    }

    public IEnumerator OpenDoor() {
        float currentRotation = 0;
        while (currentRotation < 90) {
            pivot.transform.Rotate(0, -1, 0);
            currentRotation += 1;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator CloseDoor() {
        float currentRotation = 90;
        while (currentRotation > 0) {
            pivot.transform.Rotate(0, 1, 0);
            currentRotation -= 1;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
