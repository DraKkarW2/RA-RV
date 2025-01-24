using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleFollowMovement : MonoBehaviour
{
    [Header("References")]
    public Transform leftController;
    public Transform rightController;

    [Header("Settings")]
    public Vector3 positionOffset;
    //public float followSpeed = 10f; 

    private Vector3 targetPosition;

    private void Update()
    {
        if (leftController == null || rightController == null)
        {
            Debug.LogWarning("Left or Right controller is not assigned.");
            return;
        }

        Vector3 averagePosition = (leftController.position + rightController.position) / 2f;

        targetPosition = averagePosition + positionOffset;

        //transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        AdjustRotation();
    }

    private void AdjustRotation()
    {
        Vector3 forwardDirection = rightController.position - leftController.position;
        if (forwardDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
        }
    }
}
