using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CoffeeDispenserController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem coffeeParticles; // Particle System for simulating coffee flow
    public GameObject coffeeCupPrefab; // Prefab of the coffee cup
    public Transform cupSpawnPoint; // Spawn point for the cup
    public TextMeshProUGUI distributorText; // Text UI to display messages
    public Collider interactionZone; // Zone de trigger autour de la machine

    [Header("Settings")]
    public float fillTime = 5f; // Duration to fill the cup
    public InputActionReference serveCoffeeAction; // Input action for VR button (XRI RightHand Interaction/Button)

    private GameObject spawnedCup; // Reference to the spawned cup
    private bool isFilling = false;
    private bool playerInZone = false; // Vérifie si le joueur est dans la zone

    void Start()
    {
        Debug.Log("CoffeeDispenserController started...");

        if (coffeeParticles == null)
            Debug.LogError("[Error] coffeeParticles is NOT assigned!");
        else
            Debug.Log("[OK] coffeeParticles assigned successfully.");

        if (coffeeCupPrefab == null)
            Debug.LogError("[Error] coffeeCupPrefab is NOT assigned!");
        else
            Debug.Log("[OK] coffeeCupPrefab assigned successfully.");

        if (cupSpawnPoint == null)
            Debug.LogError("[Error] cupSpawnPoint is NOT assigned!");
        else
            Debug.Log("[OK] cupSpawnPoint assigned successfully.");

        if (distributorText == null)
            Debug.LogError("[Error] distributorText is NOT assigned!");
        else
            Debug.Log("[OK] distributorText assigned successfully.");

        if (interactionZone == null)
            Debug.LogError("[Error] interactionZone is NOT assigned!");
        else
            Debug.Log("[OK] interactionZone assigned successfully.");

        if (distributorText != null)
        {
            distributorText.text = "Appuyez sur le distributeur pour obtenir du café.";
        }

        // Attache l'action d'entrée pour détecter la pression du bouton VR
        serveCoffeeAction.action.performed += _ => TryServeCoffee();
    }

    void Update()
    {
        // Vérification du clavier pour les tests, seulement si le joueur est dans la zone
        if (playerInZone && Keyboard.current.spaceKey.wasPressedThisFrame && !isFilling)
        {
            Debug.Log("Interaction detected via keyboard: Starting coffee process...");
            StartCoroutine(ServeCoffee());
        }
    }

    private void TryServeCoffee()
    {
        if (playerInZone && !isFilling)
        {
            Debug.Log("Interaction detected via VR button: Starting coffee process...");
            StartCoroutine(ServeCoffee());
        }
    }

    private IEnumerator ServeCoffee()
    {
        Debug.Log("Starting ServeCoffee process...");

        if (coffeeParticles == null || coffeeCupPrefab == null || cupSpawnPoint == null || distributorText == null)
        {
            Debug.LogError("Missing references in CoffeeDispenserController. Please assign all fields in the Inspector.");
            yield break;
        }

        isFilling = true;

        distributorText.text = "Préparation en cours...";
        Debug.Log("Text updated to: 'Préparation en cours...'");

        if (spawnedCup == null)
        {
            spawnedCup = Instantiate(coffeeCupPrefab, cupSpawnPoint.position, cupSpawnPoint.rotation);
            Debug.Log("Coffee cup spawned at position: " + cupSpawnPoint.position);
        }

        coffeeParticles.Play();
        Debug.Log("Particles started.");

        yield return new WaitForSeconds(fillTime);

        coffeeParticles.Stop();
        Debug.Log("Particles stopped.");

        distributorText.text = "Votre café est prêt !";
        Debug.Log("Text updated to: 'Votre café est prêt !'");

        yield return new WaitForSeconds(3f);
        distributorText.text = "Appuyez sur le distributeur pour obtenir du café.";
        Debug.Log("Text reset.");

        isFilling = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Player entered the coffee zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("Player left the coffee zone.");
        }
    }

    private void OnDestroy()
    {
        // Nettoyage de l'abonnement à l'événement
        serveCoffeeAction.action.performed -= _ => TryServeCoffee();
    }
}
