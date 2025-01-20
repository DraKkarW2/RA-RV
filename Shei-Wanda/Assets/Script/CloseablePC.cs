using UnityEngine;
using UnityEngine.XR;

public class CloseablePC : MonoBehaviour
{
    private bool isClosed = false;  // V�rifie si le PC est d�j� ferm�
    private bool playerInRange = false;  // V�rifie si le joueur est proche du PC

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
            Debug.Log("Le joueur s'est �loign� du PC.");
        }
    }

    void CloseComputer()
    {
        if (!isClosed)
        {
            isClosed = true;
            Debug.Log("PC ferm� !");

            // D�sactiver l'objet ou jouer une animation de fermeture
            gameObject.SetActive(false);

            // Appel � QuestManager pour mettre � jour la progression de la qu�te
            if (QuestManager.instance != null)
            {
                QuestManager.instance.ClosePC();
                Debug.Log("Appel � QuestManager pour incr�menter le compteur des PC ferm�s.");
            }
            else
            {
                Debug.LogError("QuestManager instance non trouv�e!");
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
