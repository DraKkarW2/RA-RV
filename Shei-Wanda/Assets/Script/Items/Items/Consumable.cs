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

    private Inventory inventory;
    public void SetInventory(Inventory playerInventory)
    {
        inventory = playerInventory;
    }

    [SerializeField] private Player player;
    [SerializeField] private DoorInteraction doorInteraction;   // Select the Exit Door

    public override void Use(InputAction.CallbackContext context, bool isLeftHand)
    {
        if (NumberOfUses > 0)
        {
            NumberOfUses--;
            Debug.Log($"{Name} used in {(isLeftHand ? "left" : "right")} hand, {NumberOfUses} uses left.");
            OnUse(ItemType);

            if (NumberOfUses <= 0)
            {
                Debug.Log($"{Name} is depleted and will be destroyed.");
                inventory.RemoveItem(this);
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
        switch (itemType.ToLower())
        {
            case "coffee":
                if (player != null)
                {
                    player.Sanity += 25;
                    StartCoroutine(MaxStamina(20f));
                    Debug.Log("COFFEE USED");
                }
                break;

            case "selecto":
                if (player != null)
                {
                    player.Sanity += 35;
                    StartCoroutine(MaxStamina(20f));
                    Debug.Log("SELECTO USED");
                }
                break;

            case "sandwich":
                if (player != null)
                {
                    player.Health += 50;
                    Debug.Log("SANDWICH USED");
                }
                break;

            case "battery":
                if (player != null)
                {
                    player.Battery += 100;
                    Debug.Log("BATTERY USED");
                }
                break;

            case "key":
                if (player != null)
                {
                    UseKey();
                    Debug.Log("KEY USED");
                }
                break;

            default:
                Debug.Log("Type d'item inconnu.");
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
            doorInteraction.alwaysClosed = false; // Déverrouille la porte
            Debug.Log("The door is now unlocked and can be opened.");
        }
        else
        {
            Debug.LogWarning("No door assigned to unlock with the key.");
        }
    }
}
