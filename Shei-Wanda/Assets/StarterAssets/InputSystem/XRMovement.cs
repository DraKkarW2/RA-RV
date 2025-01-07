using UnityEngine;
using UnityEngine.InputSystem;

public class XRMovement : MonoBehaviour
{
    public CharacterController characterController; // Le Character Controller attaché au XR Origin
    public Transform vrCamera; // La caméra principale pour orienter les mouvements
    public float speed = 2f; // Vitesse de déplacement
    public float gravity = -9.81f; // Gravité

    private Vector2 moveInput; // Entrée du joystick gauche
    private Vector3 velocity; // Gérer la gravité

    public void OnMove(InputValue value)
    {
        // Capture les valeurs du joystick gauche
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // Calculer la direction de mouvement basée sur la caméra
        Vector3 move = vrCamera.forward * moveInput.y + vrCamera.right * moveInput.x;
        move.y = 0f; // Pas de mouvement vertical
        characterController.Move(move * speed * Time.deltaTime);

        // Appliquer la gravité
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
