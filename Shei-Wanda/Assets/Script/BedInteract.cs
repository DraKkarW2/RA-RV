using UnityEngine;
using UnityEngine.InputSystem;

public class BedInteract : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private InputActionProperty interactButton;
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space;

    [Header("Interaction Settings")]
    public bool oneTimeUse = true; // Si on ne peut "dormir" qu'une fois
    private bool hasSlept = false;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && !hasSlept && IsInteractionPressed())
        {
            FindBed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("[BedInteract] Joueur proche du lit. Appuyez sur A/Space pour dormir.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("[BedInteract] Joueur s'est éloigné du lit.");
        }
    }

    /// <summary>
    /// Le joueur dort dans le lit -> incrémente la quête dans QuestManager.
    /// </summary>
    void FindBed()
    {
        // Logique "dormir dans le lit"
        Debug.Log("[BedInteract] Le joueur dort dans le lit...");

        // Marquer comme "utilisé" si oneTimeUse
        if (oneTimeUse)
        {
            hasSlept = true;
        }

        // Appel au QuestManager
        if (QuestManager.instance != null)
        {
            QuestManager.instance.SleepInBed();
            Debug.Log("[BedInteract] Appel à QuestManager pour incrémenter la quête du lit.");
        }
        else
        {
            Debug.LogError("[BedInteract] QuestManager instance non trouvée !");
        }
    }

    bool IsInteractionPressed()
    {
        if (interactButton.action != null && interactButton.action.WasPressedThisFrame())
        {
            Debug.Log("[BedInteract] Bouton d'interaction pressé.");
            return true;
        }
        return Input.GetKeyDown(keyboardKey);
    }
}
