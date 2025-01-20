using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private InputActionReference useItem;
    public abstract void Use(InputAction.CallbackContext context);

    private void OnEnable()
    {
        if (useItem != null)
        {
            useItem.action.Enable();
            useItem.action.performed += Use;
        }
    }

    private void OnDisable()
    {
        if (useItem != null)
        {
            useItem.action.performed -= Use;
            useItem.action.Disable();
        }
    }
}