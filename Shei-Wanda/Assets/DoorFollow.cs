using UnityEngine;

public class DoorFollowFixed : MonoBehaviour
{
    public Transform handleTransform; // Transform de la poignée
    private HingeJoint hinge;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        // Vérifie si la poignée est attachée et ajuste l'angle du Hinge Joint
        if (handleTransform != null && hinge != null)
        {
            // Calcule l'angle relatif basé sur la rotation de la poignée
            Vector3 handleLocalRotation = handleTransform.localEulerAngles;

            // Applique l'angle uniquement sur l'axe Z pour correspondre au Hinge Joint
            float angle = Mathf.Clamp(handleLocalRotation.z, hinge.limits.min, hinge.limits.max);

            // Applique la rotation en respectant les limites du Hinge Joint
            hinge.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}
