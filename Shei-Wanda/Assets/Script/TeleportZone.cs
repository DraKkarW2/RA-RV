using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportZone : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform entryPosition;  // Position à l'entrée du casier
    [SerializeField] private Transform hidePosition;   // Position à l'intérieur du casier
    [SerializeField] private Transform exitPosition;   // Position de sortie du casier

    [Header("UI Elements")]
    [SerializeField] private Canvas exitCanvas;        // Canvas avec le message

    [Header("Input Settings")]
    [SerializeField] private KeyCode exitKey = KeyCode.Space;      // Touche pour sortir
    [SerializeField] private InputActionProperty exitButton;       // Bouton A du joystick droit

    [Header("XR Movement Components")]
    public ActionBasedContinuousMoveProvider MoveProvider; // XR Movement

    private GameObject xrRig;
    private bool isHidden = false;

    void Start()
    {
        // Initialisation de xrRig
        xrRig = GameObject.Find("XR Origin (XR Rig)");
        if (xrRig == null)
        {
            Debug.LogError("XR Rig not found in the scene!");
            return;
        }

        // Désactiver le Canvas de sortie
        if (exitCanvas != null)
        {
            exitCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Exit Canvas is not assigned in the Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérification si le joueur entre dans la zone
        if (other.CompareTag("Player") && !isHidden)
        {
            StartCoroutine(TeleportToHide());
        }
    }

    void Update()
    {
        // Vérifier la sortie lorsque le joueur est caché
        if (isHidden && (Input.GetKeyDown(exitKey) || exitButton.action.WasPressedThisFrame()))
        {
            StartCoroutine(TeleportToExit());
        }
    }

    private IEnumerator TeleportToHide()
    {
        if (xrRig == null || hidePosition == null) yield break;

        // Désactiver les mouvements
        SetMovementEnabled(false);

        yield return null;

        // Téléportation
        xrRig.transform.position = hidePosition.position;
        xrRig.transform.rotation = hidePosition.rotation;
        isHidden = true;

        // Activer le Canvas de sortie
        if (exitCanvas != null)
        {
            exitCanvas.gameObject.SetActive(true);
        }

        Debug.Log("Joueur caché dans le casier");
    }

    private IEnumerator TeleportToExit()
    {
        if (xrRig == null || exitPosition == null) yield break;

        // Réactiver les mouvements
        SetMovementEnabled(true);

        yield return null;

        // Téléportation
        xrRig.transform.position = exitPosition.position;
        xrRig.transform.rotation = exitPosition.rotation;
        isHidden = false;

        // Désactiver le Canvas de sortie
        if (exitCanvas != null)
        {
            exitCanvas.gameObject.SetActive(false);
        }

        Debug.Log("Joueur sorti du casier");
    }

    private void SetMovementEnabled(bool enabled)
    {
        if (MoveProvider != null)
            MoveProvider.enabled = enabled;
    }
}
