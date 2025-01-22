using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class Item : MonoBehaviour
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _name = value;
            else
                Debug.LogWarning("Name cannot be null or empty.");
        }
    }
    private string _type;
    public string ItemType
    {
        get => _type;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _type = value;
            else
                Debug.LogWarning("Name cannot be null or empty.");
        }
    }

    [Header("Items")]
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private InputActionReference leftUseItem;
    [SerializeField] private InputActionReference rightUseItem;

    private XRGrabInteractable grabInteractable;
    private bool isCurrentlyGrabbed = false;

    [Header("Add Items to Inventory")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private InputActionReference addToInventoryAction;

    public abstract void Use(InputAction.CallbackContext context, bool isLeftHand);

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogWarning("Item requires an XRGrabInteractable component.");
        }
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed += AddToInventory;
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        if (isCurrentlyGrabbed && leftUseItem != null)
        {
            leftUseItem.action.performed -= context => Use(context, true);
        }
        if (isCurrentlyGrabbed && rightUseItem != null)
        {
            rightUseItem.action.performed -= context => Use(context, false);
        }

        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed -= AddToInventory;
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isCurrentlyGrabbed = true;
        if (leftUseItem != null)
        {
            leftUseItem.action.performed += context => Use(context, true);
        }
        if (rightUseItem != null)
        {
            rightUseItem.action.performed += context => Use(context, false);
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isCurrentlyGrabbed = false;
        if (leftUseItem != null)
        {
            leftUseItem.action.performed -= context => Use(context, true);
        }
        if (rightUseItem != null)
        {
            rightUseItem.action.performed -= context => Use(context, false);
        }
    }

    private void AddToInventory(InputAction.CallbackContext context)
    {
        if (isCurrentlyGrabbed && playerInventory != null)
        {
            bool added = playerInventory.AddItem(this);
            if (added)
            {
                isCurrentlyGrabbed = false;
                grabInteractable.enabled = false; // Empêche de grab l'item déjà dans l'inventaire

            }
        }
    }
}
