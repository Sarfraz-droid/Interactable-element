using UnityEngine;
using UnityEngine.InputSystem;

public class IE_Draggable : MonoBehaviour
{
    public bool isDraggable = false;
    Rigidbody rb;

    public InputAction inputAction;

    Vector3 targetPosition;
    public float followSpeed = 12f;
    public InputAction mouseX, mouseY;

    float dist;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if(isDraggable) {
            HandleInput();
            rb.linearVelocity = (targetPosition - transform.position) * followSpeed;
        }
    }

    void HandleInput() {
        if(inputAction == null && !isDraggable) return;

        InputControl control = inputAction.activeControl;

        if(control != null) {
            float xValue = mouseX.ReadValue<float>();
            float yValue = mouseY.ReadValue<float>();

            Vector3 mousePosition = new Vector3(Screen.width/2,Screen.height/2,
                dist);
            
            targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        } else {
            SetNotDraggable();
        }
    }


    public void SetDraggable(InteractionContent interactionContent)
    {
        dist = Vector3.Distance(transform.position, Camera.main.transform.position);

        inputAction = InputSystemUtils.GetInputAction(interactionContent.inputActionReference);
        inputAction.Enable();

        this.isDraggable = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void SetNotDraggable()
    {
        inputAction.Disable();
        inputAction = null;

        this.isDraggable = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }
}
