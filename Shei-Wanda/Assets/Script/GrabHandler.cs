using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public HingeJoint doorHinge;

    public void OnGrabStart()
    {
        Debug.Log("Handle grabbed!");
        if (doorHinge != null)
        {
            // V�rifie si la porte est proche de sa position d'origine avant de d�sactiver le ressort
            if (doorHinge.angle > -1f && doorHinge.angle < 1f)
            {
                doorHinge.useSpring = false; // Permet la manipulation si la porte est ferm�e
            }
        }
    }

    public void OnGrabEnd()
    {
        Debug.Log("Handle released!");
        if (doorHinge != null)
        {
            doorHinge.useSpring = true; // R�active le ressort pour ramener la porte
        }
    }
}
