using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class SignPresenceSheet : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject signatureCanvas;  // Canvas à afficher sur la feuille
    public TextMeshProUGUI signatureText;  // Texte qui affichera "Signé"

    [Header("Input Actions")]
    [SerializeField] private InputActionProperty interactButton;  // OpenXR joystick action
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;  // Touche espace

    private bool playerInRange = false;
    private bool isSigned = false;

    void Start()
    {
        // Cache le texte de la signature au démarrage
        if (signatureCanvas != null)
        {
            signatureCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && !isSigned && IsInteractButtonPressed())
        {
            SignSheet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Le joueur est proche de la feuille de présence.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Le joueur s'est éloigné de la feuille.");
        }
    }

    void SignSheet()
    {
        if (signatureCanvas != null && signatureText != null)
        {
            signatureCanvas.SetActive(true);
            signatureText.text = "Signé";  // Affichage du texte "Signé"
            isSigned = true;

            // Met à jour la progression de la quête via le QuestManager
            QuestManager.instance.CollectPresenceSheet();

            Debug.Log("Feuille de présence signée !");
        }
    }

    bool IsInteractButtonPressed()
    {
        if (interactButton.action != null && interactButton.action.WasPressedThisFrame())
        {
            return true;
        }
        return Input.GetKeyDown(keyboardKey);
    }
}
