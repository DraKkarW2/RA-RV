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
            case "cofee":
                Debug.Log("COFFEE USED");
                break;
            case "sandwich":
                Debug.Log("SANDWICH USED");
                break;
            case "battery":
                Debug.Log("BATTERY USED");
                break;
            default:
                Debug.Log("Type d'item inconnu.");
                break;
        }
    }
}
