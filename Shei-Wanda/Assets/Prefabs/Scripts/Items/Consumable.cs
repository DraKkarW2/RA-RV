using UnityEngine;

public class Consumable : Item
{
    // Attributs sp�cifiques aux consommables
    public int NumberUsage;

    // M�thodes sp�cifiques
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
