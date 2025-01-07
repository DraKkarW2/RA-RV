using UnityEngine;
using UnityEngine.InputSystem;

public class XRMovement : MonoBehaviour
{
    public CharacterController characterController; // Le Character Controller attach� au XR Origin
    public Transform vrCamera; // La cam�ra principale pour orienter les mouvements
    public float speed = 2f; // Vitesse de d�placement
    public float gravity = -9.81f; // Gravit�

    private Vector2 moveInput; // Entr�e du joystick gauche
    private Vector3 velocity; // G�rer la gravit�

    public void OnMove(InputValue value)
    {
        // Capture les valeurs du joystick gauche
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // Calculer la direction de mouvement bas�e sur la cam�ra
        Vector3 move = vrCamera.forward * moveInput.y + vrCamera.right * moveInput.x;
        move.y = 0f; // Pas de mouvement vertical
        characterController.Move(move * speed * Time.deltaTime);

        // Appliquer la gravit�
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
