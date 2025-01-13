using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public HingeJoint doorHinge;

    public void OnGrabStart()
    {
        Debug.Log("Handle grabbed!");
        // Ajoute ici des comportements spécifiques si nécessaire.
    }

    public void OnGrabEnd()
    {
        Debug.Log("Handle released!");
        // Ajoute ici des comportements spécifiques si nécessaire.
    }
}
