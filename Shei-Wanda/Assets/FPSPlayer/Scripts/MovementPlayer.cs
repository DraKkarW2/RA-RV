using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayer : MonoBehaviour
{
    public float speed = 3.0f;
    public InputActionProperty moveInput; 
    public Transform cameraTransform;

    private CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>();

        Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        Vector3 direction = forward * input.y + right * input.x;

        characterController.Move(direction * speed * Time.deltaTime);
    }
}
