using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class LockerTeleport : MonoBehaviour
{
    [Header("References")]
    public Transform insideLockerPosition;  // Position � l'int�rieur du casier
    public Transform outsideLockerPosition; // Position de sortie du casier
    public GameObject exitTextUI;           // UI pour afficher le message de sortie
    public CharacterController playerController; // R�f�rence au CharacterController du joueur
    public GameObject playerCamera; // R�f�rence � la cam�ra du joueur
    public XROrigin xrOrigin; // XR Origin rig for teleportation
    public GameObject playerObject; // Le GameObject du joueur (assigner 'playerDefault')

    private bool isInside = false;          // V�rifie si le joueur est � l'int�rieur du casier

    void Start()
    {
        // D�sactiver le texte UI au d�but
        if (exitTextUI != null)
        {
            exitTextUI.SetActive(false);
        }
        else
        {
            Debug.LogError("[LockerTeleport] Exit Text UI is not assigned in the Inspector!");
        }

        // V�rifie si playerObject est assign�, sinon cherche automatiquement le joueur
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
            // D�place le XR Origin � la position du casier et tourne la cam�ra
            xrOrigin.MoveCameraToWorldLocation(insideLockerPosition.position);
            playerObject.transform.rotation = Quaternion.Euler(0, insideLockerPosition.eulerAngles.y + 180, 0);

            // D�sactiver le mouvement du joueur
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
            // D�place le XR Origin � la position de sortie
            xrOrigin.MoveCameraToWorldLocation(outsideLockerPosition.position);

            // R�activer le mouvement du joueur
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
