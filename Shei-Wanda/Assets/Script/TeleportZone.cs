using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] private Transform entryPosition;  // Position à l'entrée du casier
    [SerializeField] private Transform hidePosition;  // Position à l'intérieur du casier
    [SerializeField] private Transform exitPosition;  // Position de sortie du casier
    [SerializeField] private Canvas exitCanvas;       // Canvas avec le message
    [SerializeField] private KeyCode exitKey = KeyCode.Space;  // Touche pour sortir
    [SerializeField] private InputActionProperty exitButton;  // Bouton A du joystick droit

    private GameObject xrRig;
    private CharacterController characterController;
    private Rigidbody capsuleRigidbody;
    private bool isHidden = false;

    void Start()
    {
        xrRig = GameObject.Find("XR Origin (XR Rig)");
        if (xrRig != null)
        {
            characterController = xrRig.GetComponent<CharacterController>();
            capsuleRigidbody = xrRig.transform.Find("Capsule").GetComponent<Rigidbody>();
            if (capsuleRigidbody != null)
            {
                capsuleRigidbody.isKinematic = true;  // Assurer qu'il est en cinématique
            }
        }
        else
        {
            Debug.LogError("XR Rig not found in the scene!");
        }

        if (exitCanvas != null)
        {
            exitCanvas.gameObject.SetActive(false);  // Désactiver le message au début
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHidden)
        {
            StartCoroutine(TeleportToHide());
        }
    }

    void Update()
    {
        if (isHidden && (Input.GetKeyDown(exitKey) || exitButton.action.WasPressedThisFrame()))
        {
            StartCoroutine(TeleportToExit());
        }
    }

    private IEnumerator TeleportToHide()
    {
        if (xrRig == null || hidePosition == null) yield break;

        if (characterController != null)
            characterController.enabled = false;

        if (capsuleRigidbody != null)
        {
            capsuleRigidbody.isKinematic = true;
            capsuleRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        }

        yield return null;

        xrRig.transform.position = hidePosition.position;
        xrRig.transform.rotation = hidePosition.rotation;
        isHidden = true;

        if (exitCanvas != null)
        {
            exitCanvas.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        if (characterController != null)
            characterController.enabled = true;

        Debug.Log("Joueur caché dans le casier");
    }

    private IEnumerator TeleportToExit()
    {
        if (xrRig == null || exitPosition == null) yield break;

        if (characterController != null)
            characterController.enabled = false;

        if (capsuleRigidbody != null)
        {
            capsuleRigidbody.isKinematic = true;
            capsuleRigidbody.constraints = RigidbodyConstraints.None;
        }

        yield return null;

        xrRig.transform.position = exitPosition.position;
        xrRig.transform.rotation = exitPosition.rotation;
        isHidden = false;

        if (exitCanvas != null)
        {
            exitCanvas.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.1f);

        if (characterController != null)
            characterController.enabled = true;

        Debug.Log("Joueur sorti du casier");
    }
}
