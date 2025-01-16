using UnityEngine;

public class Equipment : Item
{
    // Attributs sp�cifiques aux �quipements
    public Vector3 Range; // Port�e de l'�quipement
    public int Battery;
    public bool IsCharged;

    // M�thodes sp�cifiques
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
