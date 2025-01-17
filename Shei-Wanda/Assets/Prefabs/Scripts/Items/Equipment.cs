using UnityEngine;

public class Equipment : Item
{
    // Attributs spécifiques aux équipements
    public Vector3 Range; // Portée de l'équipement
    public int Battery;
    public bool IsCharged;

    // Méthodes spécifiques
    public override void Use()
    {
        if (IsCharged)
        {
            Debug.Log($"{Name} is being used.");
        }
        else
        {
            Debug.Log($"{Name} is not charged and cannot be used.");
        }
    }

    public void ChargeBattery(int amount)
    {
        Battery += amount;
        if (Battery > 0)
        {
            IsCharged = true;
        }

        Debug.Log($"{Name} charged. Battery: {Battery}");
    }
}
