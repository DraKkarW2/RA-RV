using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class CoffeeDispenserController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem coffeeParticles; // Particle System for simulating coffee flow
    public GameObject coffeeCupPrefab; // Prefab of the coffee cup
    public Transform cupSpawnPoint; // Spawn point for the cup
    public TextMeshProUGUI distributorText; // Text UI to display messages

    [Header("Settings")]
    public float fillTime = 5f; // Duration to fill the cup

    private GameObject spawnedCup; // Reference to the spawned cup
    private bool isFilling = false;
    private bool playerInRange = false; // Check if the player is near the dispenser

    void Start()
    {
        Debug.Log("CoffeeDispenserController started...");

        // Debug checks for inspector references
        if (coffeeParticles == null)
            Debug.LogError("[Error] coffeeParticles is NOT assigned! Please assign it in the Inspector.");
        else
            Debug.Log("[OK] coffeeParticles assigned successfully.");

        if (coffeeCupPrefab == null)
            Debug.LogError("[Error] coffeeCupPrefab is NOT assigned! Please assign it in the Inspector.");
        else
            Debug.Log("[OK] coffeeCupPrefab assigned successfully.");

        if (cupSpawnPoint == null)
            Debug.LogError("[Error] cupSpawnPoint is NOT assigned! Please assign it in the Inspector.");
        else
            Debug.Log("[OK] cupSpawnPoint assigned successfully.");

        if (distributorText == null)
            Debug.LogError("[Error] distributorText is NOT assigned! Please assign it in the Inspector.");
        else
            Debug.Log("[OK] distributorText assigned successfully.");

        // Set default text if assigned
        if (distributorText != null)
        {
            distributorText.text = "Appuyez sur A pour obtenir du café.";
        }
    }

    void Update()
    {
        if (playerInRange && !isFilling)
        {
            // Detect interaction with keyboard
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Interaction detected: Starting coffee process...");
                StartCoroutine(ServeCoffee());
            }

            // Detect VR interaction (Meta Quest 3 - OpenXR, Button A)
            if (IsVRButtonPressed())
            {
                Debug.Log("VR Interaction detected: Starting coffee process...");
                StartCoroutine(ServeCoffee());
            }
        }
    }

    bool IsVRButtonPressed()
    {
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool primaryButtonPressed = false;

        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed)
        {
            return true;
        }
        return false;
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
        distributorText.text = "Appuyez sur A pour obtenir du café.";
        Debug.Log("Text reset.");

        isFilling = false;
    }

    // Detection of the player entering the interaction zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered the coffee dispenser area.");
        }
    }

    // Detection of the player leaving the interaction zone
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left the coffee dispenser area.");
        }
    }
}
