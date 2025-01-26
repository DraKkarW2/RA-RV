using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class LoadSceneOnPoke : MonoBehaviour
{
    public string sceneToLoad = "Menu"; // Nom de la scène à charger

    private void Start()
    {
        // Vérifier si le bouton a un XR Poke Interactable
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnPoke);
        }
    }

    private void OnPoke(SelectEnterEventArgs args)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
