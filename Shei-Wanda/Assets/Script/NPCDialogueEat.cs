using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogueEat : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;  // Canvas UI du dialogue (toujours actif)
    public TextMeshProUGUI dialogueText;  // Texte pour afficher le dialogue (affichage dynamique)
    public TextMeshProUGUI interactionText;  // Texte d'invite "Appuyer sur A..." (toujours visible)

    [Header("Dialogue Data")]
    public string[] dialogueLines;  // Liste des dialogues
    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;
    private bool transactionHandled = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private AudioClip dialogueSound;

    [Header("Input Settings")]
    [SerializeField] private InputActionProperty closeButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    [Header("Transaction Settings")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Item itemToSell;
    [SerializeField] private int itemPrice = 2;
    [SerializeField] private Player player;

    void Start()
    {
        dialogueCanvas.SetActive(true);  // Le canvas reste toujours activé
        dialogueText.gameObject.SetActive(false);  // Masquer le texte de dialogue au départ
        interactionText.gameObject.SetActive(true);  // Toujours visible
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
            if (!transactionHandled)
            {
                HandleTransaction();
                transactionHandled = true;
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.gameObject.SetActive(false);  // Cacher le texte de dialogue, mais garder interactionText visible
        currentLineIndex = 0;
        transactionHandled = false;
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

    void HandleTransaction()
    {
        if (player != null && playerInventory != null && itemToSell != null)
        {
            if (player.Money >= itemPrice)
            {
                bool added = playerInventory.AddItem(Instantiate(itemToSell));

                if (added)
                {
                    player.Money -= itemPrice;
                    Debug.Log($"Le joueur a acheté {itemToSell.Name} pour {itemPrice}€. Argent restant : {player.Money}€");

                    dialogueText.text = $"Merci pour votre achat ! Vous avez acheté un {itemToSell.Name}.\n" +
                                       $"Argent restant : {player.Money}€";
                }
                else
                {
                    Debug.LogWarning("Inventaire plein, impossible d'acheter l'objet.");
                    dialogueText.text = "Votre inventaire est plein, revenez plus tard.";
                }
            }
            else
            {
                Debug.LogWarning("Pas assez d'argent pour acheter cet objet.");
                dialogueText.text = "Désolé, vous n'avez pas assez d'argent.";
            }
        }
    }
}
