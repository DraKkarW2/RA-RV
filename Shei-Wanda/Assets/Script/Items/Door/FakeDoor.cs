using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FakeDoor : MonoBehaviour
{
    public AudioClip screamerSound;  // Le son du screamer
    private AudioSource audioSource;
    private bool hasActivated = false;

    public XRGrabInteractable doorHandle; // Référence à la poignée de porte

    void Start()
    {
        // Vérifie et ajoute un AudioSource si non assigné
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (doorHandle != null)
        {
            doorHandle.selectEntered.AddListener(OnHandleGrabbed);
        }
        else
        {
            Debug.LogError("Door handle is not assigned in the Inspector!");
        }
    }

    void OnHandleGrabbed(SelectEnterEventArgs args)
    {
        if (!hasActivated)
        {
            TriggerScreamer();
        }
    }

    void TriggerScreamer()
    {
        hasActivated = true;

        if (screamerSound != null)
        {
            audioSource.PlayOneShot(screamerSound);
            Debug.Log("Screamer déclenché !");
        }
        else
        {
            Debug.LogError("Aucun son assigné à la porte !");
        }
    }

    void OnDestroy()
    {
        if (doorHandle != null)
        {
            doorHandle.selectEntered.RemoveListener(OnHandleGrabbed);
        }
    }
}
