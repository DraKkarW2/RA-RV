using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public HingeJoint doorHinge;

    public void OnGrabStart()
    {
        Debug.Log("Handle grabbed!");
        if (doorHinge != null)
        {
            // Vérifie si la porte est proche de sa position d'origine avant de désactiver le ressort
            if (doorHinge.angle > -1f && doorHinge.angle < 1f)
            {
                doorHinge.useSpring = false; // Permet la manipulation si la porte est fermée
            }
        }
    }

    public void OnGrabEnd()
    {
        Debug.Log("Handle released!");
        if (doorHinge != null)
        {
            doorHinge.useSpring = true; // Réactive le ressort pour ramener la porte
        }
    }
}
