using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public HingeJoint doorHinge;

    public void OnGrabStart()
    {
        Debug.Log("Handle grabbed!");
        // Ajoute ici des comportements sp�cifiques si n�cessaire.
    }

    public void OnGrabEnd()
    {
        Debug.Log("Handle released!");
        // Ajoute ici des comportements sp�cifiques si n�cessaire.
    }
}
