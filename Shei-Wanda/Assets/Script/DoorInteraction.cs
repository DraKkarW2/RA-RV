using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private InputActionProperty closeButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    public Transform door;  // Référence à la porte à ouvrir
    public float openAngle = 90f;  // Angle maximal d'ouverture
    public float openSpeed = 2f;  // Vitesse d'ouverture

    private bool isOpening = false;
    private bool canOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = door.rotation;
        targetRotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y + openAngle, door.eulerAngles.z);
    }

    void Update()
    {
        // Vérification si le bouton d'interaction est pressé (VR ou clavier)
        if (canOpen && (closeButton.action.WasPressedThisFrame() || Input.GetKeyDown(keyboardKey)))
        {
            isOpening = true;
        }

        // Si ouverture activée, interpoler la rotation
        if (isOpening)
        {
            door.rotation = Quaternion.Lerp(door.rotation, targetRotation, Time.deltaTime * openSpeed);

            // Arrêter l'ouverture quand l'angle est proche de la cible
            if (Quaternion.Angle(door.rotation, targetRotation) < 1f)
            {
                isOpening = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
}
