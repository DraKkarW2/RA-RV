using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;  // Canvas UI du dialogue (toujours actif)
    public TextMeshProUGUI dialogueText;  // Texte pour afficher le dialogue (affichage dynamique)
    public TextMeshProUGUI interactionText;  // Texte d'invite "Appuyer sur A..." (toujours visible)
    public GameObject questCanvas;  // UI pour la quête
    public TextMeshProUGUI questText;  // Texte pour "PC Fermés : 0/10"

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
        dialogueCanvas.SetActive(true);  // Toujours visible
        dialogueText.gameObject.SetActive(false);  // Masquer le texte du dialogue au début
        interactionText.gameObject.SetActive(true);  // Toujours afficher le texte d'interaction
        questCanvas.SetActive(false);  // Cacher la quête au début
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
        dialogueText.gameObject.SetActive(true);  // Afficher le texte du dialogue
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
            StartQuest();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.gameObject.SetActive(false);  // Cacher le texte du dialogue après la fin
        currentLineIndex = 0;
    }

    void StartQuest()
    {
        questCanvas.SetActive(true);

        if (QuestManager.instance != null)
        {
            QuestManager.instance.UpdateQuestText();  // Mise à jour du texte sans incrémenter
            Debug.Log("Quête de fermeture des PC démarrée !");
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
