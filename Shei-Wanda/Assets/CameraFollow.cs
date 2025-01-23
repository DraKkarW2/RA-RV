using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public Vector3 positionOffset;

    void Update()
    {
        Vector3 averagePosition = (leftController.position + rightController.position) / 2f;

        float minHeight = 0.37f; // Hauteur minimale pour la caméra (par exemple, hauteur des yeux)
        averagePosition.y = Mathf.Max(averagePosition.y, minHeight);

        transform.position = averagePosition + positionOffset;
    }
}
