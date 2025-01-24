using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Consumable : Item
{
    [SerializeField]
    private int numberOfUses = 1;

    public int NumberOfUses
    {
        get => numberOfUses;
        set
        {
            if (value >= 0)
                numberOfUses = value;
            else
                Debug.LogWarning("Number of uses cannot be negative.");
        }
    }

    public string itemType; // Type d'objet consommable
    private Inventory inventory; // Inventaire associé

    [SerializeField] private Player player; // Référence au joueur
    [SerializeField] private DoorInteraction doorInteraction; // Référence à l'interaction de la porte

    public void SetInventory(Inventory playerInventory)
    {
        inventory = playerInventory;
    }

    public override void Use(InputAction.CallbackContext context, bool isLeftHand)
    {
        if (NumberOfUses > 0)
        {
            if (!string.IsNullOrEmpty(itemType))
            {
                OnUse(itemType); // Utilisation de l'objet
            }
            else
            {
                Debug.LogWarning("Item type is not set.");
                return;
            }

            Debug.Log($"{Name} used in {(isLeftHand ? "left" : "right")} hand, {NumberOfUses} uses left.");

            if (NumberOfUses <= 0)
            {
                Debug.Log($"{Name} is depleted and will be destroyed.");
                if (inventory != null)
                {
                    inventory.RemoveItem(this);
                }
                else
                {
                    Debug.LogWarning("Inventory is not assigned.");
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log($"{Name} is depleted and cannot be used.");
        }
    }

    private void OnUse(string itemType)
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned. Cannot use item.");
            return;
        }

        switch (itemType.ToLower())
        {
            case "coffee":
                player.Sanity += 25;
                StartCoroutine(MaxStamina(20f));
                NumberOfUses--;
                Debug.Log("COFFEE USED");
                break;

            case "selecto":
                player.Sanity += 35;
                StartCoroutine(MaxStamina(40f));
                NumberOfUses--;
                Debug.Log("SELECTO USED");
                break;

            case "sandwich":
                player.Health += 50;
                NumberOfUses--;
                Debug.Log("SANDWICH USED");
                break;

            case "battery":
                player.Battery += 100;
                NumberOfUses--;
                Debug.Log("BATTERY USED");
                break;

            case "key":
                UseKey();
                break;

            default:
                Debug.LogWarning("Unknown item type.");
                break;
        }
    }

    private IEnumerator MaxStamina(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (player.Stamina != 100)
                player.Stamina = 100;

            yield return null; // Attends la prochaine frame avant de continuer
        }
    }

    private void UseKey()
    {
        if (doorInteraction != null)
        {
            if (doorInteraction.alwaysClosed)
            {
                doorInteraction.alwaysClosed = false; // Déverrouille la porte
                Debug.Log("The door is now unlocked and can be opened.");
                NumberOfUses--;
            }
            else
            {
                Debug.LogWarning("The door is already unlocked.");
            }
        }
        else
        {
            Debug.LogWarning("No door assigned to unlock with the key.");
        }
    }
}
