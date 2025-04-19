using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public InputAction moveAction;
    public InputAction mouseX, mouseY;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        moveAction.Enable();
        mouseX.Enable();
        mouseY.Enable();

        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    Vector2 GetMove(InputAction inputAction) {
        Vector2 movement = inputAction.ReadValue<Vector2>();

        return movement;    
    }

    Vector2 GetLook(InputAction inputAction) {
        Vector2 look = inputAction.ReadValue<Vector2>();

        return look;
    }

    float GetFloat(InputAction inputAction) {
        float value = inputAction.ReadValue<float>();

        return value;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = 0, curSpeedY = 0;
            Vector2 movement = GetMove(moveAction);
            curSpeedX = canMove ? movement.x * (isRunning ? runningSpeed : walkingSpeed) : 0;
            curSpeedY = canMove ? movement.y * (isRunning ? runningSpeed : walkingSpeed) : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            Vector2 Look;

            float mouseXValue = GetFloat(mouseX);
            float mouseYValue = GetFloat(mouseY);

            Look = new Vector2(mouseXValue, mouseYValue);

            rotationX += -Look.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Look.x * lookSpeed, 0);
        }
    }
}