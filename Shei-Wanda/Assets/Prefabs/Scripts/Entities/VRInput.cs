using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class VRInput : MonoBehaviour
{
    public Vector2 moveInput; 
    public Vector2 lookInput; 


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}
