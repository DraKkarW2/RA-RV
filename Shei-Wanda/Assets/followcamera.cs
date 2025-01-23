using UnityEngine;

public class CapsuleFollowControllers : MonoBehaviour
{
    public Transform leftController;  
    public Transform rightController; 
    public Vector3 positionOffset; 

    void Update()
    {
        Vector3 averagePosition = (leftController.position + rightController.position) / 2f;

        transform.position = averagePosition + positionOffset;
    }
}