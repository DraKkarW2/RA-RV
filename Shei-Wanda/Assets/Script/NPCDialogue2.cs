using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCDialogue2 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;  // Canvas UI du dialogue (toujours actif)
    public TextMeshProUGUI dialogueText;  // Texte pour afficher le dialogue (affichage dynamique)
    public TextMeshProUGUI interactionText;  // Texte d'invite "Appuyer sur A..." (toujours visible)
    public GameObject questCanvas;  // Canvas pour afficher la quête
    public TextMeshProUGUI questText;  // Texte de la quête des feuilles de présence

    [Header("Dialogue Data")]
    public string[] dialogueLines;  // Liste des dialogues
    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private AudioClip dialogueSound;

    [SerializeField] private InputActionProperty closeButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    void Start()
    {
        dialogueCanvas.SetActive(true);  // Le canvas est toujours visible
        dialogueText.gameObject.SetActive(false);  // Masquer le texte de dialogue au départ
        interactionText.gameObject.SetActive(true);  // Toujours afficher le texte d'interaction
        questCanvas.SetActive(false);  // Cache la quête au début
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
            Debug.Log("Joueur entré dans la zone du PNJ !");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (dialogueActive)
            {
                EndDialogue();
            }
            Debug.Log("Joueur a quitté la zone du PNJ !");
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        dialogueText.gameObject.SetActive(true);  // Afficher le texte de dialogue
        dialogueText.text = dialogueLines[currentLineIndex];
        PlayDialogueSound();
    }

    public void AdvanceDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            PlayDialogueSound();
        }
        else
        {
            EndDialogue();
            StartPresenceSheetQuest();  // Lancer la quête après le dialogue
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.gameObject.SetActive(false);  // Masquer le texte de dialogue après la fin
        currentLineIndex = 0;
    }

    void StartPresenceSheetQuest()
    {
        questCanvas.SetActive(true);

        if (QuestManager.instance != null)
        {
            QuestManager.instance.UpdatePresenceSheetQuestText();
            Debug.Log("Quête de signature des feuilles de présence démarrée !");
        }
        else
        {
            Debug.LogError("QuestManager instance not found! Ensure it's added to the scene.");
        }
    }

    void PlayDialogueSound()
    {
        if (dialogueAudioSource != null && dialogueSound != null)
        {
            dialogueAudioSource.PlayOneShot(dialogueSound);
        }
    }

    bool IsAButtonPressed()
    {
        if (closeButton.action != null && closeButton.action.WasPressedThisFrame())
        {
            return true;
        }
        return Input.GetKeyDown(keyboardKey);
    }
}
