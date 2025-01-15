using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSanity : MonoBehaviour
{
    public float sanity = 100f; 
    public float sanityDecreaseRate = 0.1f; 
    public float sanityDecreaseInDarkness = 0.5f;
    // public Image sanityBar; 

    public AudioSource sanitySound;
    private bool isInDarkness = false;

    public GameObject fakeObjectPrefab; 
    public float fakeObjectDuration = 1f; // Modifier la durée


    public void TriggerSanityDecrease(float amount)
    {
        sanity -= amount;
        //UpdateSanityBar();
    }

    void Update()
    {
        if (!isInDarkness)
        {
            sanity -= sanityDecreaseRate * Time.deltaTime;
        }
        else
        {
            sanity -= sanityDecreaseInDarkness * Time.deltaTime;
        }

        if (sanity < 0)
        {
            sanity = 0;
            PlayerDeath();
        }

        if (sanity <= 20f)
        {
            if (!sanitySound.isPlaying)
            {
                sanitySound.Play();
            }
        }
        else
        {
            if (sanitySound.isPlaying)
            {
                sanitySound.Stop();
            }
        }

        if (sanity <= 20f && Random.value < 0.01f) // Apparition aléatoire d'un faux objet
        {
            CreateFakeObject();
        }

        // Met à jour l'affichage de la barre de sanité
        //UpdateSanityBar();
    }

    //void UpdateSanityBar()
    //{
    //    // Met à jour la barre de sanité en fonction de la valeur actuelle
    //    sanityBar.fillAmount = sanity / 100f;
    //}

    void CreateFakeObject()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)) + transform.position;
        GameObject fakeObject = Instantiate(fakeObjectPrefab, randomPosition, Quaternion.identity);

        Destroy(fakeObject, fakeObjectDuration);
    }
    void PlayerDeath()
    {
        Debug.Log("Player is dead.");
        //Function to end the game.

        //Charger une autre scène
        //UnityEngine.SceneManagement.SceneManager.LoadScene("sceneàcharger");

    }

    public void SetInDarkness(bool inDarkness)
    {
        isInDarkness = inDarkness;
    }
}
