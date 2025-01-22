using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int maxSize = 5;                        // Taille maximale de l'inventaire
    private List<Item> items = new List<Item>();    // Liste des items dans l'inventaire
    private Item leftHandItem;
    private Item rightHandItem;

    [SerializeField]
    private Equipment leftHandEquipment;  // Référence à l'équipement de la main gauche
    [SerializeField]
    private Equipment rightHandEquipment; // Référence à l'équipement de la main droite

    // Ajoute un item à l'inventaire
    public bool AddItem(Item item)
    {
        // Gestion des stacks pour les consommables
        if (item is Consumable consumable)
        {
            foreach (var existingItem in items)
            {
                if (existingItem is Consumable existingConsumable && existingConsumable.ItemType == consumable.ItemType)
                {
                    // Ajoute le stack au consommable existant
                    existingConsumable.NumberOfUses += consumable.NumberOfUses;
                    Destroy(item.gameObject); // Supprime le nouvel item après l'ajout au stack
                    Debug.Log($"{consumable.Name} stack updated. Total uses: {existingConsumable.NumberOfUses}");
                    return true;
                }
            }
            consumable.SetInventory(this);
        }

        if (items.Count >= maxSize)
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }

        items.Add(item);
        item.gameObject.SetActive(false);
        Debug.Log($"{item.Name} added to inventory.");
        return true;
    }

    // Change l'item sélectionné dans l'inventaire (rotation)
    public void CycleItem(bool isLeftHand)
    {
        if (items.Count == 0)
        {
            // Si l'inventaire est vide, réinitialise l'état de la main
            if (isLeftHand)
            {
                leftHandItem = null;
                if (leftHandEquipment != null)
                {
                    leftHandEquipment.ResetActiveState(true);  // Réinitialise la main gauche
                }
            }
            else
            {
                rightHandItem = null;
                if (rightHandEquipment != null)
                {
                    rightHandEquipment.ResetActiveState(false);  // Réinitialise la main droite
                }
            }
            Debug.Log("Inventory is empty. Hand is now empty.");
            return;
        }

        Item currentItem = isLeftHand ? leftHandItem : rightHandItem;
        int currentIndex = items.IndexOf(currentItem);
        int nextIndex = (currentIndex + 1) % items.Count;

        Item nextItem = items[nextIndex];

        if (nextItem == (isLeftHand ? rightHandItem : leftHandItem))
        {
            nextIndex = (nextIndex + 1) % items.Count;
            nextItem = items[nextIndex];
        }

        if (isLeftHand) leftHandItem = nextItem;
        else rightHandItem = nextItem;

        Debug.Log($"{(isLeftHand ? "Left" : "Right")} hand selected {nextItem.Name}");
    }

    // Retire un item de l'inventaire
    public void RemoveItem(Item item)
    {
        if (items.Remove(item))
        {
            Debug.Log($"{item.Name} removed from inventory.");
        }

        if (leftHandItem == item) leftHandItem = null;
        if (rightHandItem == item) rightHandItem = null;
    }
}