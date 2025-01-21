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

    public override void Use(InputAction.CallbackContext context)
    {
        if (NumberOfUses > 0)
        {
            NumberOfUses--;
            Debug.Log($"{Name} used, {NumberOfUses} uses left.");
            OnUse(ItemType);

            if (NumberOfUses <= 0)
            {
                Debug.Log($"{Name} is depleted and will be destroyed.");
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log($"{Name} is depleted and cannot be used.");
        }
    }

    public void UpdateItem()
    {
        Debug.Log($"{Name} updated. Number of uses: {NumberOfUses}");
    }

    private void OnUse(string itemType)
    {
        switch (itemType)
        {
            case "Coffee":
                Debug.Log("COFFEE USED");
                break;
            default:
                Debug.Log("Type d'item inconnu.");
                break;
        }
    }
}