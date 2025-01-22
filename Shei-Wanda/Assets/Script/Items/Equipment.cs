using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : Item
{
    [SerializeField]
    private bool isActiveLeft = false;
    private bool isActiveRight = false;

    // M�thodes sp�cifiques
    public override void Use(InputAction.CallbackContext context, bool isLeftHand)
    {
        if (isLeftHand)
        {
            // Inverse l'�tat d'activation pour la main gauche
            isActiveLeft = !isActiveLeft;
            if (isActiveLeft)
                Activate(isLeftHand);
            else
                Deactivate(isLeftHand);
        }
        else
        {
            // Inverse l'�tat d'activation pour la main droite
            isActiveRight = !isActiveRight;
            if (isActiveRight)
                Activate(isLeftHand);
            else
                Deactivate(isLeftHand);
        }
    }

    // M�thode appel�e lors de l'activation de l'�quipement
    private void Activate(bool isLeftHand)
    {
        switch (ItemType.ToLower())
        {
            case "flashlight":
                Debug.Log("flashlight ACTIVATED");
                break;
            default:
                Debug.Log("Type d'�quipement inconnu lors de l'activation.");
                break;
        }
    }

    private void Deactivate(bool isLeftHand)
    {
        switch (ItemType.ToLower())
        {
            case "flashlight":
                Debug.Log("flashlight DESACTIVATED");
                break;
            default:
                Debug.Log("Type d'�quipement inconnu lors de la d�sactivation.");
                break;
        }
    }

    // Fonction pour r�initialiser l'�tat 'isActive' quand il n'y a pas d'objet grab
    public void ResetActiveState(bool isLeftHand)
    {
        if (isLeftHand)
        {
            if (isActiveLeft)
            {
                isActiveLeft = false;
                Debug.Log($"{Name} (left hand) state reset to inactive.");
            }
        }
        else
        {
            if (isActiveRight)
            {
                isActiveRight = false;
                Debug.Log($"{Name} (right hand) state reset to inactive.");
            }
        }
    }
}