using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public List<Item> Items = new List<Item>(); 
    public int HealthBar; 
    public int SanityBar; 
    public int StaminaBar; 
    public int MoneyCount; 
    public int BatteryBar; 

    public bool HasEquipment; 
    public bool InvStatus; 

   

  
    public void Show(bool status)
    {
        InvStatus = status;
        if (status)
        {
            Debug.Log("Inventory is now open.");
        }
        else
        {
            Debug.Log("Inventory is now closed.");
        }
    }


    public void UpdateInventory()
    {
        Debug.Log("Inventory updated.");
    }

    public void SelectInventory(Item item)
    {
        if (Items.Contains(item))
        {
            Debug.Log($"Selected item: {item.Name}");
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }


    public void AddItem(Item item)
    {
        Items.Add(item);
        Debug.Log($"Added {item.Name} to inventory.");
        UpdateInventory();
    }

   
    public void RemoveItem(Item item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            Debug.Log($"Removed {item.Name} from inventory.");
            UpdateInventory();
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }

    public void UseItem(Item item)
    {
        if (Items.Contains(item))
        {
            item.Use();
            Debug.Log($"Used {item.Name}.");
            UpdateInventory();
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }
}
