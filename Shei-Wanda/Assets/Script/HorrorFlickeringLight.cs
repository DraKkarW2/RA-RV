using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class HorrorFlickeringLight : MonoBehaviour
{
    public Light lightSource; // La lumière à contrôler
    public float minIntensity = 0.2f; // Intensité minimale
    public float maxIntensity = 2.0f; // Intensité maximale
    public float flickerSpeed = 0.1f; // Vitesse du clignotement
    public bool enableFlickerSound = true; // Activer/désactiver le son de clignotement
    public AudioSource audioSource; // Source audio pour le son
    public AudioClip flickerMusic; // Musique pour les lumières qui clignotent
    public AudioClip lightsOffMusic; // Musique pour les lumières éteintes

    private float targetIntensity;
    private float timer;
    private static List<HorrorFlickeringLight> allLights = new List<HorrorFlickeringLight>();
    private static bool flickerStarted = false;
    private static bool lightsOffStarted = false;
    private bool isFlickering = false; // Ajout pour suivre l'état de clignotement de chaque lampe

    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }
        targetIntensity = lightSource.intensity;
        allLights.Add(this);
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (elapsedTime >= 20f && !flickerStarted) // 5 minutes
        {
            StartFlickering();
            flickerStarted = true;
            if (audioSource != null && flickerMusic != null && !audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.clip = flickerMusic;
                audioSource.Play();
            }
        }

        if (elapsedTime >= 40f && !lightsOffStarted) // 10 minutes
        {
            TurnOffSomeLights();
            lightsOffStarted = true;
            if (audioSource != null && lightsOffMusic != null)
            {
                audioSource.loop = false;
                audioSource.PlayOneShot(lightsOffMusic);
            }
        }

        if (isFlickering)
        {
            timer += Time.deltaTime;
            if (timer >= flickerSpeed)
            {
                targetIntensity = Random.Range(minIntensity, maxIntensity);
                timer = 0;
            }
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, Time.deltaTime * 10);
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = $"Time : {minutes:00}:{seconds:00}";
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    void StartFlickering()
    {
        int numberOfLightsToFlicker = Mathf.CeilToInt(allLights.Count * 0.35f);
        HashSet<int> selectedIndexes = new HashSet<int>();

        while (selectedIndexes.Count < numberOfLightsToFlicker)
        {
            int index = Random.Range(0, allLights.Count);
            if (!selectedIndexes.Contains(index))
            {
                allLights[index].isFlickering = true;
                selectedIndexes.Add(index);
            }
        }
    }

    void TurnOffSomeLights()
    {
        int numberOfLightsToTurnOff = Mathf.CeilToInt(allLights.Count * 0.2f);
        HashSet<int> selectedIndexes = new HashSet<int>();

        while (selectedIndexes.Count < numberOfLightsToTurnOff)
        {
            int index = Random.Range(0, allLights.Count);
            if (!selectedIndexes.Contains(index))
            {
                allLights[index].lightSource.enabled = false;
                selectedIndexes.Add(index);
            }
        }
    }
}
