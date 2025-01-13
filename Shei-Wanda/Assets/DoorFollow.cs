using UnityEngine;

public class DoorFollow : MonoBehaviour
{
    public Transform handleTransform; // Transform de la poignée

    private HingeJoint hinge;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        // Synchronise la rotation de la porte avec la poignée si besoin
        Vector3 handleRotation = handleTransform.localEulerAngles;
        hinge.transform.localEulerAngles = new Vector3(0, 0, handleRotation.z);
    }
}
