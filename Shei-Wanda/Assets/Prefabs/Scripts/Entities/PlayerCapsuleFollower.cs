using UnityEngine;

public class CapsuleFollower : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin; // Référence à l'XR Origin (le rig du joueur)
    [SerializeField] private CapsuleCollider capsule; // Capsule Collider pour suivre la position du joueur

    private void Update()
    {
        if (xrOrigin == null || capsule == null)
        {
            Debug.LogWarning("XR Origin or Capsule is not assigned.");
            return;
        }

        // Mettre à jour la position de la capsule pour qu'elle suive l'XR Origin
        Vector3 xrPosition = xrOrigin.position;
        capsule.transform.position = new Vector3(xrPosition.x, xrPosition.y + (capsule.height / 2), xrPosition.z);

        // Mettre à jour la rotation de la capsule pour correspondre à celle de l'XR Origin
        capsule.transform.rotation = xrOrigin.rotation;
    }
}
