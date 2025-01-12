using UnityEngine;

public class ScreenHingeRotation : MonoBehaviour
{
    public Transform screen; // R�f�rence � l'�cran
    public float minAngle = 0f; // Angle ouvert (�cran lev�)
    public float maxAngle = 110f; // Angle ferm� (�cran abaiss�)
    public float rotationSpeed = 5f; // Vitesse d'interpolation si besoin

    private bool isBeingGrabbed = false;

    void Update()
    {
        if (!isBeingGrabbed)
        {
            // S'assurer que l'�cran reste dans les limites d'angle
            float clampedAngle = Mathf.Clamp(screen.localEulerAngles.x, maxAngle, minAngle);
            screen.localEulerAngles = new Vector3(clampedAngle, screen.localEulerAngles.y, screen.localEulerAngles.z);
        }
    }

    // Appel� lorsqu'on commence � grab
    public void OnGrabStart()
    {
        isBeingGrabbed = true;
    }

    // Appel� lorsqu'on rel�che
    public void OnGrabEnd()
    {
        isBeingGrabbed = false;
    }
}
