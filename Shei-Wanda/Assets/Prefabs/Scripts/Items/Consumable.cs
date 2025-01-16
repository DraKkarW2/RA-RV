using UnityEngine;

public class Consumable : Item
{
    // Attributs spécifiques aux consommables
    public int NumberUsage;

    // Méthodes spécifiques
    public override void Use()
    {
        if (NumberUsage > 0)
        {
            NumberUsage--;
            Debug.Log($"{Name} used, {NumberUsage} usages left.");
        }
        else
        {
            Debug.Log($"{Name} is depleted and cannot be used.");
        }
    }

    public void UpdateItem()
    {
        Debug.Log($"{Name} updated. Number of usages: {NumberUsage}");
    }
}
