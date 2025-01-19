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
    public GameObject playerCamera; // Référence à la caméra du joueur
    public XROrigin xrOrigin; // XR Origin rig for teleportation

    private bool isInside = false;          // Vérifie si le joueur est à l'intérieur du casier
    private GameObject player;              // Référence au joueur

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
        if (other.CompareTag("Player") && !isInside)
        {
            player = other.gameObject;
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
        if (player != null && xrOrigin != null)
        {
            // Utiliser XR Origin pour téléporter le joueur
            xrOrigin.MoveCameraToWorldLocation(insideLockerPosition.position);
            player.transform.rotation = Quaternion.Euler(0, insideLockerPosition.eulerAngles.y + 180, 0);

            // Désactiver le mouvement du joueur
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            isInside = true;
            Debug.Log("Player entered the locker and rotated 180 degrees.");

            // Afficher l'instruction pour sortir
            if (exitTextUI != null)
            {
                exitTextUI.SetActive(true);
            }
        }
    }

    void ExitLocker()
    {
        if (player != null && xrOrigin != null)
        {
            // Utiliser XR Origin pour téléporter le joueur à l'extérieur
            xrOrigin.MoveCameraToWorldLocation(outsideLockerPosition.position);

            // Réactiver le mouvement du joueur
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            Debug.Log("Player exited the locker and kept the current rotation.");
            isInside = false;

            // Cacher l'instruction de sortie
            if (exitTextUI != null)
            {
                exitTextUI.SetActive(false);
            }
        }
    }
}
