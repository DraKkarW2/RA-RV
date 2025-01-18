using UnityEngine;

public class ScreenHingeRotation : MonoBehaviour
{
    public Transform screen; // Référence à l'écran
    public float minAngle = 0f; // Angle ouvert (écran levé)
    public float maxAngle = 110f; // Angle fermé (écran abaissé)
    public float rotationSpeed = 5f; // Vitesse d'interpolation si besoin

    private bool isBeingGrabbed = false;

    void Update()
    {
        if (!isBeingGrabbed)
        {
            // S'assurer que l'écran reste dans les limites d'angle
            float clampedAngle = Mathf.Clamp(screen.localEulerAngles.x, maxAngle, minAngle);
            screen.localEulerAngles = new Vector3(clampedAngle, screen.localEulerAngles.y, screen.localEulerAngles.z);
        }
    }

    // Appelé lorsqu'on commence à grab
    public void OnGrabStart()
    {
        isBeingGrabbed = true;
    }

    // Appelé lorsqu'on relâche
    public void OnGrabEnd()
    {
        isBeingGrabbed = false;
    }
}
