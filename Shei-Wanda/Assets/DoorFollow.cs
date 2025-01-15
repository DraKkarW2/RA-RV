using UnityEngine;

public class DoorFollowFixed : MonoBehaviour
{
    public Transform handleTransform; // Transform de la poign�e
    private HingeJoint hinge;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        // V�rifie si la poign�e est attach�e et ajuste l'angle du Hinge Joint
        if (handleTransform != null && hinge != null)
        {
            // Calcule l'angle relatif bas� sur la rotation de la poign�e
            Vector3 handleLocalRotation = handleTransform.localEulerAngles;

            // Applique l'angle uniquement sur l'axe Z pour correspondre au Hinge Joint
            float angle = Mathf.Clamp(handleLocalRotation.z, hinge.limits.min, hinge.limits.max);

            // Applique la rotation en respectant les limites du Hinge Joint
            hinge.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}
