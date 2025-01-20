using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class LockerTeleport : MonoBehaviour
{
    [Header("References")]
    public Transform insideLockerPosition;  // Position à l'intérieur du casier
    public Transform outsideLockerPosition; // Position de sortie du casier
    public GameObject exitTextUI;           // UI pour afficher le message de sortie
    public CharacterController playerController; // Référence au CharacterController du joueur
    public GameObject playerCamera; // Référence à la caméra du joueur
    public XROrigin xrOrigin; // XR Origin rig for teleportation
    public GameObject playerObject; // Le GameObject du joueur (assigner 'playerDefault')

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
            Debug.LogError("[LockerTeleport] Exit Text UI is not assigned in the Inspector!");
        }

        // Vérifie si playerObject est assigné, sinon cherche automatiquement le joueur
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                Debug.LogError("[LockerTeleport] Player not found! Ensure 'playerDefault' has the correct tag.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject && !isInside)
        {
            Debug.Log("[LockerTeleport] Player detected: Teleporting...");
            TeleportToLocker();
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
            // Déplace le XR Origin à la position du casier et tourne la caméra
            xrOrigin.MoveCameraToWorldLocation(insideLockerPosition.position);
            playerObject.transform.rotation = Quaternion.Euler(0, insideLockerPosition.eulerAngles.y + 180, 0);

            // Désactiver le mouvement du joueur
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            isInside = true;
            Debug.Log("[LockerTeleport] Player entered the locker and rotated 180 degrees.");

            // Afficher l'instruction pour sortir
            if (exitTextUI != null)
            {
                exitTextUI.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("[LockerTeleport] Teleportation failed. Check player and XR Origin references.");
        }
    }

    void ExitLocker()
    {
        if (playerObject != null && xrOrigin != null)
        {
            // Déplace le XR Origin à la position de sortie
            xrOrigin.MoveCameraToWorldLocation(outsideLockerPosition.position);

            // Réactiver le mouvement du joueur
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            Debug.Log("[LockerTeleport] Player exited the locker.");
            isInside = false;

            // Cacher l'instruction de sortie
            if (exitTextUI != null)
            {
                exitTextUI.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("[LockerTeleport] Exit failed. Check player and XR Origin references.");
        }
    }
}
