using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : MonoBehaviour
{
    public Transform leftController; 
    public Transform rightController;

    [Header("Settings")]
    public Vector3 positionOffset; 

    private void Update()
    {
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Left or Right controller is not assigned.");
            return;
        }
        
            Vector3 averagePosition = (leftController.position + rightController.position) / 2f;

        transform.position = averagePosition + positionOffset;
    }
}