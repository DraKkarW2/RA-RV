using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;  // Canvas UI du dialogue
    public TextMeshProUGUI dialogueText;  // Texte pour afficher le dialogue
    public TextMeshProUGUI interactionText;  // Texte d'invite "Appuyer sur A..."
    public GameObject questCanvas;  // UI pour la qu�te
    public TextMeshProUGUI questText;  // Texte pour "PC Ferm�s : 0/10"

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
        // Activer le canvas de la qu�te dans l'UI joueur
        questCanvas.SetActive(true);

        // Initialiser la qu�te de fermeture des PC via le QuestManager existant
        if (QuestManager.instance != null)
        {
            QuestManager.instance.ClosePC(); // D�clenche la mise � jour initiale du texte "PC ferm�s : 0/10"
            Debug.Log("Qu�te de fermeture des PC d�marr�e !");
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
