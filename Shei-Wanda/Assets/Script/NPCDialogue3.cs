using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCDialogue3 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI interactionText;
    public GameObject questCanvas;
    public TextMeshProUGUI questText;

    [Header("Dialogue Data")]
    [Tooltip("Dialogue à afficher AVANT que la quête soit terminée.")]
    public string[] dialogueLinesBeforeQuest;

    [Tooltip("Dialogue à afficher APRES que la quête du lit soit terminée.")]
    public string[] dialogueLinesAfterQuest;

    private string[] currentDialogueLines;
    private bool isUsingAfterQuestLines = false; // Pour savoir si on utilise le "après quete"
    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private AudioClip dialogueSound;

    [Header("Input Settings")]
    [SerializeField] private InputActionProperty closeButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    [Header("Item Reward Settings")]
    [SerializeField] private Inventory playerInventory;   // Inventaire du joueur
    [SerializeField] private Item itemToGive;            // L'objet à donner
    [SerializeField] private Player player;              // Référence au script Player (si besoin)
    private bool rewardGiven = false; // Pour éviter de donner plusieurs fois l'objet

    void Start()
    {
        // Détermine quelles lignes utiliser en fonction de l'état de la quête
        SetDialogueLinesBasedOnQuestStatus();

        dialogueCanvas.SetActive(true);        // Toujours visible
        dialogueText.gameObject.SetActive(false);  // Masquer le texte au début
        interactionText.gameObject.SetActive(true);
        questCanvas.SetActive(false);          // Masquer la quête au début
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

            // Met à jour le dialogue en cas de changement de l'état de la quête
            SetDialogueLinesBasedOnQuestStatus();
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

    // --------------------- //
    //       DIALOGUE        //
    // --------------------- //

    void StartDialogue()
    {
        dialogueActive = true;
        dialogueText.gameObject.SetActive(true);  // Afficher le texte
        dialogueText.text = currentDialogueLines[currentLineIndex];
        PlayDialogueSound();
    }

    public void AdvanceDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex < currentDialogueLines.Length)
        {
            // Continue le dialogue
            dialogueText.text = currentDialogueLines[currentLineIndex];
            PlayDialogueSound();
        }
        else
        {
            // Fin du tableau de lignes
            EndDialogue();

            // Si on est dans les lignes "après la quête" et qu'on n'a pas encore donné l'objet, on le donne
            if (isUsingAfterQuestLines && !rewardGiven)
            {
                GiveItemToPlayer();
                rewardGiven = true;
            }

            // Si la quête n'est pas terminée, on la lance
            if (QuestManager.instance != null && !QuestManager.instance.IsBedQuestCompleted())
            {
                StartQuest();
            }
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.gameObject.SetActive(false);
        currentLineIndex = 0;
    }

    // --------------------- //
    //         QUETE         //
    // --------------------- //

    void StartQuest()
    {
        questCanvas.SetActive(true);

        if (QuestManager.instance != null)
        {
            QuestManager.instance.UpdateBedQuestText();
            Debug.Log("Quête de trouver un lit démarré!");
        }
        else
        {
            Debug.LogError("QuestManager instance not found! Assurez-vous de l'avoir dans la scène.");
        }
    }

    // --------------------- //
    //      GESTION ITEM     //
    // --------------------- //

    /// <summary>
    /// Donne l'item "itemToGive" au joueur, si l'inventaire le permet.
    /// </summary>
    void GiveItemToPlayer()
    {
        if (playerInventory != null && itemToGive != null)
        {
            // Essaye d'ajouter l'item dans l'inventaire du joueur
            bool added = playerInventory.AddItem(Instantiate(itemToGive));

            if (added)
            {
                Debug.Log($"[NPC] Objet '{itemToGive.Name}' donné au joueur !");

                // Optionnel : Vous pouvez afficher un message de confirmation dans le dialogue
                //   ou mettre à jour la UI d'une autre façon.
                dialogueCanvas.SetActive(true);
                dialogueText.gameObject.SetActive(true);
                dialogueText.text = $"Vous avez reçu : {itemToGive.Name} !";
            }
            else
            {
                Debug.LogWarning("[NPC] Inventaire plein, impossible de donner l'objet.");
                dialogueCanvas.SetActive(true);
                dialogueText.gameObject.SetActive(true);
                dialogueText.text = "Votre inventaire est plein, impossible de recevoir l'objet.";
            }
        }
    }

    // --------------------- //
    //   CHOIX LIGNES DIAL.  //
    // --------------------- //

    /// <summary>
    /// Selon l'état de la quête, on choisit les lignes de dialogue "avant" ou "après" la quête.
    /// </summary>
    void SetDialogueLinesBasedOnQuestStatus()
    {
        if (QuestManager.instance != null && QuestManager.instance.IsBedQuestCompleted())
        {
            // La quête est terminée -> on passe au dialogue "post-quête"
            currentDialogueLines = dialogueLinesAfterQuest;
            isUsingAfterQuestLines = true;
        }
        else
        {
            // La quête n'est pas terminée -> on utilise le dialogue "avant-quête"
            currentDialogueLines = dialogueLinesBeforeQuest;
            isUsingAfterQuestLines = false;
        }
    }

    // --------------------- //
    //   OUTILS ET SONS      //
    // --------------------- //

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
