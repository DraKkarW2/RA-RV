using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenVRRotation : MonoBehaviour
{
    public Transform hinge; // Charnière pour la rotation
    public float minAngle = 0f; // Angle minimum (ouvert)
    public float maxAngle = 110f; // Angle maximum (fermé)
    public float rotationSpeed = 35f; // Vitesse de rotation

    private bool isGrabbed = false;

    void Update()
    {
        if (isGrabbed)
        {
            // Rotation de l'écran en fonction du mouvement de la main
            float rotationInput = Input.GetAxis("Vertical"); // Ajuste selon l'entrée VR
            float currentAngle = hinge.localEulerAngles.x;
            float targetAngle = Mathf.Clamp(currentAngle + rotationInput * rotationSpeed * Time.deltaTime, maxAngle, minAngle);
            hinge.localRotation = Quaternion.Euler(targetAngle, hinge.localEulerAngles.y, hinge.localEulerAngles.z);
        }
    }

    public void OnGrabStart()
    {
        isGrabbed = true;
    }

    public void OnGrabEnd()
    {
        isGrabbed = false;
    }
}
