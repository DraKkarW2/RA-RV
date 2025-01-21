using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class LockerTeleport : MonoBehaviour
{
    public Transform insideLockerPosition;  // Position à l'intérieur du casier
    public Transform outsideLockerPosition; // Position de sortie du casier
    public GameObject exitTextUI;           // UI pour afficher le message de sortie
    public CharacterController playerController; // Référence au CharacterController du joueur
    public GameObject playerCamera;         // Référence à la caméra du joueur
    public XROrigin xrOrigin;               // XR Origin rig pour la téléportation
    public GameObject playerObject;         // Référence au GameObject du joueur

    private bool isInside = false;          // Vérifie si le joueur est à l'intérieur du casier

    void Start()
    {
        // Désactiver le texte UI au début
        if (exitTextUI != null)
        {
            exitTextUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Exit Text UI is not assigned in the Inspector!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in locker zone.");
            TeleportToLocker();
        }
        else
        {
            Debug.Log("Non-player object detected: " + other.gameObject.name);
        }
    }

    void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            ExitLocker();
        }

        // VR Interaction: Appuyer sur le bouton A du joystick (Meta Quest 3 - OpenXR)
        if (isInside && IsVRButtonPressed())
        {
            ExitLocker();
        }
    }

    bool IsVRButtonPressed()
    {
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool primaryButtonPressed = false;

        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed)
        {
            return true;
        }
        return false;
    }

    void TeleportToLocker()
    {
        if (playerObject != null && xrOrigin != null)
        {
            Vector3 newPosition = insideLockerPosition.position + new Vector3(0, 0.1f, 0); // Légère élévation pour éviter de tomber à travers le sol
            xrOrigin.MoveCameraToWorldLocation(newPosition);
            playerObject.transform.rotation = Quaternion.Euler(0, insideLockerPosition.eulerAngles.y + 180, 0);

            // Désactiver le CharacterController pour empêcher les déplacements
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            // Désactiver le Rigidbody pour empêcher tout mouvement
            Rigidbody rb = playerObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Désactiver les scripts de déplacement
            LocomotionSystem locomotion = playerObject.GetComponent<LocomotionSystem>();
            if (locomotion != null)
            {
                locomotion.enabled = false;
            }

            isInside = true;
            Debug.Log("Player entered the locker and rotated 180 degrees.");

            // Afficher l'instruction pour sortir
            if (exitTextUI != null)
            {
                exitTextUI.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Teleport failed: Player object or XR Origin is not assigned.");
        }
    }

    void ExitLocker()
    {
        if (playerObject != null && xrOrigin != null)
        {
            xrOrigin.MoveCameraToWorldLocation(outsideLockerPosition.position);

            // Réactiver le CharacterController pour permettre le déplacement
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            // Réactiver le Rigidbody
            Rigidbody rb = playerObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Réactiver les scripts de déplacement
            LocomotionSystem locomotion = playerObject.GetComponent<LocomotionSystem>();
            if (locomotion != null)
            {
                locomotion.enabled = true;
            }

            Debug.Log("Player exited the locker.");
            isInside = false;

            // Cacher l'instruction de sortie
            if (exitTextUI != null)
            {
                exitTextUI.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Exit failed: Player object or XR Origin is not assigned.");
        }
    }
}
