using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using TMPro;

public class ScreenVRRotation : MonoBehaviour
{
    public Transform hinge; // Charnière pour la rotation
    public float minAngle = 0f;  // Angle ouvert
    public float maxAngle = 110f; // Angle fermé
    public float rotationSpeed = 35f; // Vitesse de rotation
    public TextMeshProUGUI questText;  // UI pour afficher la progression

    private XRGrabInteractable grabInteractable; // XR Grab Interactable
    private bool isGrabbed = false;
    private bool isClosed = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabStart);
            grabInteractable.selectExited.AddListener(OnGrabEnd);
        }
    }

    void Update()
    {
        if (isGrabbed && !isClosed)
        {
            // Lire l'entrée du joystick de la manette VR
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            Vector2 axisValue;
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisValue))
            {
                float rotationInput = axisValue.y;

                // Appliquer la rotation en maintenant dans la plage autorisée
                float currentAngle = hinge.localEulerAngles.x;
                float targetAngle = Mathf.Clamp(currentAngle + rotationInput * rotationSpeed * Time.deltaTime, maxAngle, minAngle);
                hinge.localRotation = Quaternion.Euler(targetAngle, hinge.localEulerAngles.y, hinge.localEulerAngles.z);

                // Vérifier si l'écran est complètement fermé
                if (targetAngle >= maxAngle - 1)
                {
                    ClosePC();
                }
            }
        }
    }

    public void OnGrabStart(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    public void OnGrabEnd(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void ClosePC()
    {
        if (!isClosed)
        {
            isClosed = true;
            Debug.Log("PC fermé.");

            if (QuestManager.instance != null)
            {
                QuestManager.instance.ClosePC();
            }
            else
            {
                Debug.LogError("QuestManager instance is missing!");
            }
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabStart);
            grabInteractable.selectExited.RemoveListener(OnGrabEnd);
        }
    }
}
