using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventory; // Le Canvas ou UI de l'inventaire
    private bool isInventoryVisible = false; // �tat d'affichage
    public InputActionReference toggleInventoryAction; // R�f�rence � l'action pour ouvrir/fermer l'inventaire
    public Transform inventoryAnchor;
    public Vector3 positionOffset = new Vector3(10f, 0, 20f);

    private void OnEnable()
    {
        // S'abonner � l'�v�nement de l'action d'entr�e
        toggleInventoryAction.action.performed += OnToggleInventory;
    }

    private void OnDisable()
    {
        // Se d�sabonner de l'�v�nement de l'action d'entr�e
        toggleInventoryAction.action.performed -= OnToggleInventory;
    }

    private void Update()
    {
        if (isInventoryVisible)
        {
            inventory.transform.position = inventoryAnchor.position + positionOffset; 
            inventory.transform.rotation = inventoryAnchor.rotation;
        }
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }

    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventory.SetActive(isInventoryVisible);
    }
}
