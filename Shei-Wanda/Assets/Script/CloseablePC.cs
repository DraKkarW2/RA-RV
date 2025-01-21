using UnityEngine;
using UnityEngine.InputSystem;

public class CloseablePC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform screenTransform; // Référence à l'écran
    [SerializeField] private Rigidbody screenRigidbody; // Référence au Rigidbody de l'écran
    [SerializeField] private BoxCollider interactionZone;

    [Header("Controls")]
    [SerializeField] private InputActionProperty closeButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    [Header("Rotation Settings")]
    [SerializeField] private float closedAngle = 110f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private bool isClosed = false;
    private bool playerInRange = false;
    private Coroutine closingRoutine;

    void Update()
    {
        if (playerInRange && IsInteractionPressed() && !isClosed)
        {
            if (closingRoutine == null)
            {
                closingRoutine = StartCoroutine(CloseScreen());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("[CloseablePC] Le joueur est proche du PC. Appuyez sur A pour fermer.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("[CloseablePC] Le joueur s'est éloigné du PC.");
        }
    }

    System.Collections.IEnumerator CloseScreen()
    {
        Debug.Log("[CloseablePC] Fermeture de l'écran en cours...");

        isClosed = true;
        screenRigidbody.isKinematic = true;  // Désactiver la physique pour éviter les forces indésirables

        float elapsedTime = 0f;
        float startAngle = screenTransform.localEulerAngles.x;
        float targetAngle = closedAngle;
        float duration = 1.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAngle = Mathf.LerpAngle(startAngle, targetAngle, elapsedTime / duration);
            screenTransform.localRotation = Quaternion.Euler(Mathf.Clamp(newAngle, 0, closedAngle), 0, 0);
            yield return null;
        }

        screenTransform.localRotation = Quaternion.Euler(closedAngle, 0, 0);
        screenRigidbody.detectCollisions = false;  // Désactivation des collisions après fermeture

        Debug.Log("[CloseablePC] PC fermé.");

        closingRoutine = null;

        // Met à jour la progression de la quête si applicable
        if (QuestManager.instance != null)
        {
            QuestManager.instance.ClosePC();
            Debug.Log("[CloseablePC] Appel à QuestManager pour incrémenter le compteur des PC fermés.");
        }
        else
        {
            Debug.LogError("[CloseablePC] QuestManager instance non trouvée!");
        }
    }

    bool IsInteractionPressed()
    {
        if (closeButton.action.WasPressedThisFrame())
        {
            Debug.Log("[CloseablePC] Bouton A pressé.");
            return true;
        }
        return Input.GetKeyDown(keyboardKey);
    }
}
