using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventory; // Le Canvas ou UI de l'inventaire
    private bool isInventoryVisible = false; // État d'affichage
    public InputActionReference toggleInventoryAction;

    void Update()
    {
        // Remplacez "ButtonPress" par l'événement correspondant dans XR Device Simulator
        if (Input.GetKeyDown(KeyCode.I)) // Pour un test clavier avec la touche I
        {
            ToggleInventory();
        }

        if (isInventoryVisible)
        {
            inventory.transform.position = transform.position + transform.up * 0.2f; // Ajustez l'offset
            inventory.transform.rotation = transform.rotation;
        }
    }

    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventory.SetActive(isInventoryVisible);
    }

}
