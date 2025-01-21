using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class LockerHide : MonoBehaviour
{
    public Transform insidePosition;   // Point de téléportation intérieur
    public Transform outsidePosition;  // Point de sortie extérieur
    public GameObject exitCanvas;      // Canvas affichant "Appuyer sur A pour sortir"
    public InputActionProperty exitAction; // Bouton A pour sortir

    private CharacterController playerController;
    private bool isInside = false;
    private float yOffset = 0.1f;  // Décalage pour éviter les collisions avec le sol

    private void Start()
    {
        exitCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CharacterController>();
            if (playerController != null)
            {
                StartCoroutine(TeleportToLocker());
            }
        }
    }

    private IEnumerator TeleportToLocker()
    {
        if (playerController != null)
        {
            playerController.enabled = false;  // Désactiver le CharacterController temporairement

            // Ajustement précis de la position et légère élévation pour éviter la chute
            Vector3 targetPosition = insidePosition.position;
            targetPosition.y += playerController.height / 2 + yOffset;

            playerController.transform.position = targetPosition;

            // Rotation du joueur pour qu'il soit face au casier
            playerController.transform.rotation = Quaternion.Euler(0, insidePosition.eulerAngles.y + 180, 0);

            yield return new WaitForSeconds(0.1f);  // Délai pour assurer le placement correct

            playerController.enabled = true;  // Réactiver le contrôle

            exitCanvas.SetActive(true);  // Afficher l'instruction pour sortir
            isInside = true;
        }
    }

    private IEnumerator ExitLocker()
    {
        if (playerController != null)
        {
            playerController.enabled = false;  // Désactiver le CharacterController

            Vector3 targetPosition = outsidePosition.position;
            targetPosition.y += playerController.height / 2 + yOffset;

            playerController.transform.position = targetPosition;
            playerController.transform.rotation = outsidePosition.rotation;

            yield return new WaitForSeconds(0.1f);

            playerController.enabled = true;  // Réactiver le contrôle

            exitCanvas.SetActive(false);  // Cacher l'instruction de sortie
            isInside = false;
        }
    }

    private void Update()
    {
        if (isInside && exitAction.action.WasPerformedThisFrame())
        {
            StartCoroutine(ExitLocker());
        }
    }
}
