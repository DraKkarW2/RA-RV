using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float sanity = 100f; 
    public float sanityDecayRateLight = 1f; 
    public float sanityDecayRateDark = 2f; 
    public bool isInDarkness = false; 

    [Header("Game Over Settings")]
    public string gameOverScene = "Menu"; 
    private bool isDead = false; 

    [Header("Sanity Effects")]
    public AudioSource creepySoundEffect; 
    public GameObject[] fakeObjects; 
    public float fakeObjectSpawnRate = 5f; 
    private float lastFakeObjectSpawnTime;

    [Header("Communication Interference")]
    public bool disableCommunication = false; 
    public AudioSource communicationAudio;


    public GameObject playerDefault;
    public GameObject gameOverCanvas;

    private void Start()
    {
        lastFakeObjectSpawnTime = Time.time;

        
        if (fakeObjects != null)
        {
            foreach (GameObject obj in fakeObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isDead) return;

        
        float decayRate = isInDarkness ? sanityDecayRateDark : sanityDecayRateLight;
        sanity -= decayRate * Time.deltaTime;
        sanity = Mathf.Max(sanity, 0f); 

       
        HandleSanityEffects();

        
        if (sanity <= 0f)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        isDead = true;

        //Debug.Log("Player is dead. Game Over!");

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Invoke("ReturnToLobby", 3f);

        SceneManager.LoadScene(gameOverScene);
    }

    public void ReduceSanity(float amount)
    {
        sanity -= amount;
        sanity = Mathf.Max(sanity, 0f);
    }

    public void IncreaseSanity(float amount)
    {
        sanity += amount;
        sanity = Mathf.Min(sanity, 100f);
    }

    private void HandleSanityEffects()
    {
        if (sanity < 50f)
        {
            if (creepySoundEffect != null && !creepySoundEffect.isPlaying)
            {
                creepySoundEffect.Play();
            }

            if (!disableCommunication && communicationAudio != null)
            {
                communicationAudio.Play();
                disableCommunication = true;
            }

            if (fakeObjects != null && Time.time > lastFakeObjectSpawnTime + fakeObjectSpawnRate)
            {
                SpawnFakeObject();
                lastFakeObjectSpawnTime = Time.time;
            }
        }
        else
        {
            if (creepySoundEffect != null && creepySoundEffect.isPlaying)
            {
                creepySoundEffect.Stop();
            }

            if (disableCommunication && communicationAudio != null)
            {
                communicationAudio.Stop();
                disableCommunication = false;
            }
        }
    }

    private void SpawnFakeObject()
    {
        int randomIndex = Random.Range(0, fakeObjects.Length);
        GameObject obj = fakeObjects[randomIndex];
        obj.SetActive(true);

        StartCoroutine(HideFakeObject(obj, 3f));
    }

    private System.Collections.IEnumerator HideFakeObject(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
