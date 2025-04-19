using UnityEngine;

public class InteractableUtils : MonoBehaviour
{
    public float pushForce = 10f;

    public void DeleteObject() {
        Destroy(gameObject);
    }

    public void PushObject() {
        Vector3 direction = transform.position - Camera.main.transform.position;
        direction.Normalize();
        Rigidbody rb = GetComponent<Rigidbody>();
        
        if(rb != null) {
            rb.AddForce(direction * pushForce, ForceMode.Impulse);
        }
    }
}
