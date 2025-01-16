using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventory; // Le Canvas ou UI de l'inventaire
    private bool isInventoryVisible = false; // État d'affichage
    public InputActionReference toggleInventoryAction; // Référence à l'action pour ouvrir/fermer l'inventaire
    public Transform inventoryAnchor;
    public Vector3 positionOffset = new Vector3(10f, 0, 20f);

    private void OnEnable()
    {
        // S'abonner à l'événement de l'action d'entrée
        toggleInventoryAction.action.performed += OnToggleInventory;
    }

    private void OnDisable()
    {
        // Se désabonner de l'événement de l'action d'entrée
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
