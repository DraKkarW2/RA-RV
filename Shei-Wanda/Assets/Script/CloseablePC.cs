using UnityEngine;
using UnityEngine.XR;

public class CloseablePC : MonoBehaviour
{
    private bool isClosed = false;  // Vérifie si le PC est déjà fermé
    private bool playerInRange = false;  // Vérifie si le joueur est proche du PC

    void Update()
    {
        if (playerInRange && IsAButtonPressed() && !isClosed)
        {
            CloseComputer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Le joueur est proche du PC. Appuyez sur A pour fermer.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Le joueur s'est éloigné du PC.");
        }
    }

    void CloseComputer()
    {
        if (!isClosed)
        {
            isClosed = true;
            Debug.Log("PC fermé !");

            // Désactiver l'objet ou jouer une animation de fermeture
            gameObject.SetActive(false);

            // Appel à QuestManager pour mettre à jour la progression de la quête
            if (QuestManager.instance != null)
            {
                QuestManager.instance.ClosePC();
                Debug.Log("Appel à QuestManager pour incrémenter le compteur des PC fermés.");
            }
            else
            {
                Debug.LogError("QuestManager instance non trouvée!");
            }
        }
    }

    bool IsAButtonPressed()
    {
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool aButtonPressed = false;
        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed);
        return aButtonPressed;
    }
}
