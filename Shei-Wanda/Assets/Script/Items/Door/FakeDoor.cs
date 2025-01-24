using UnityEngine;
using UnityEngine.InputSystem;

public class FakeDoor : MonoBehaviour
{
    [Header("Screamer Settings")]
    public AudioClip screamerSound;  // Le son du screamer
    private AudioSource audioSource;
    private bool hasActivated = false;
    private bool playerInRange = false;  // Vérifie si le joueur est dans la zone

    [Header("Interaction Settings")]
    [SerializeField] private InputActionProperty interactButton; // Bouton du joystick VR
    [SerializeField] private KeyCode keyboardKey = KeyCode.Space; // Touche clavier pour interaction
    [SerializeField] private Collider interactionZone; // Zone d'interaction

    [SerializeField] private float scareDuration = 3.0f; // Durée du jumpscare en secondes
    [SerializeField] private float interactionCooldown = 2.0f; // Délai entre interactions

    private float lastInteractionTime = 0f; // Temps de la dernière interaction

    void Start()
    {
        // Ajout automatique d'AudioSource si nécessaire
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // Son 3D
        audioSource.volume = 0.8f; // Volume modifiable

        if (screamerSound == null)
        {
            Debug.LogError("Aucun son assigné à la porte !");
        }
    }

    void Update()
    {
        if (playerInRange && IsInteractionPressed() && !hasActivated && Time.time - lastInteractionTime > interactionCooldown)
        {
            TriggerScreamer();
            lastInteractionTime = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Le joueur est proche de la poignée de porte. Appuyez sur Espace ou Bouton A pour interagir.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Le joueur s'est éloigné de la poignée de porte.");
        }
    }

    void TriggerScreamer()
    {
        hasActivated = true;

        if (screamerSound != null)
        {
            audioSource.PlayOneShot(screamerSound);
            Debug.Log("Screamer déclenché !");

            // Réduire la sanité du joueur
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<PlayerSanity>().ReduceSanity(10);
            }

            Invoke(nameof(StopScreamer), scareDuration); // Arrêter le son après la durée définie
        }
    }


    void StopScreamer()
    {
        audioSource.Stop();
        Debug.Log("Screamer arrêté après " + scareDuration + " secondes.");
    }

    bool IsInteractionPressed()
    {
        return (interactButton.action != null && interactButton.action.WasPressedThisFrame()) || Input.GetKeyDown(keyboardKey);
    }
}
