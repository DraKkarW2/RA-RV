using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCDialogueEasterEgg : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;  // Canvas UI du dialogue
    public TextMeshProUGUI dialogueText;  // Texte pour afficher le dialogue
    public TextMeshProUGUI interactionText;  // Texte d'invite "Appuyer sur A..."
    
    

    [Header("Dialogue Data")]
    public string[] dialogueLines;  // Liste des dialogues
    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource dialogueAudioSource; // Audio source pour le son des dialogues
    [SerializeField] private AudioClip dialogueSound; // Son à jouer pour chaque dialogue

    [SerializeField] private InputActionProperty closeButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    void Start()
    {
        dialogueCanvas.SetActive(false);
        interactionText.gameObject.SetActive(false);
        
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
            if (!dialogueActive)
            {
                interactionText.gameObject.SetActive(true);
            }
            Debug.Log("Joueur entré dans la zone du PNJ !");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionText.gameObject.SetActive(false);
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
        dialogueCanvas.SetActive(true);
        dialogueText.text = dialogueLines[currentLineIndex];
        interactionText.gameObject.SetActive(false);
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
            
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueCanvas.SetActive(false);
        currentLineIndex = 0;
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
