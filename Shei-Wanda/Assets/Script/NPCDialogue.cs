using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;  // Canvas UI du dialogue
    public TextMeshProUGUI dialogueText;  // Texte pour afficher le dialogue
    public TextMeshProUGUI interactionText;  // Texte d'invite "Appuyer sur A..."
    public GameObject questCanvas;  // UI pour la quête
    public TextMeshProUGUI questText;  // Texte pour "PC Fermés : 0/10"

    [Header("Dialogue Data")]
    public string[] dialogueLines;  // Liste des dialogues
    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Start()
    {
        dialogueCanvas.SetActive(false);
        interactionText.gameObject.SetActive(false);
        questCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && IsAButtonPressed())
        {
            if (!dialogueActive)
            {
                StartDialogue();
            }
            else
            {
                AdvanceDialogue();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionText.gameObject.SetActive(false);
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        dialogueCanvas.SetActive(true);
        dialogueText.text = dialogueLines[currentLineIndex];
        interactionText.gameObject.SetActive(false);
    }

    public void AdvanceDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue();
            StartQuest();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueCanvas.SetActive(false);
        currentLineIndex = 0;
    }

    void StartQuest()
    {
        // Activer le canvas de la quête dans l'UI joueur
        questCanvas.SetActive(true);

        // Initialiser la quête de fermeture des PC via le QuestManager existant
        if (QuestManager.instance != null)
        {
            QuestManager.instance.ClosePC(); // Déclenche la mise à jour initiale du texte "PC fermés : 0/10"
            Debug.Log("Quête de fermeture des PC démarrée !");
        }
        else
        {
            Debug.LogError("QuestManager instance not found! Ensure it's added to the scene.");
        }
    }


    bool IsAButtonPressed()
    {
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool aButtonPressed = false;
        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed);
        return aButtonPressed;
    }
}
