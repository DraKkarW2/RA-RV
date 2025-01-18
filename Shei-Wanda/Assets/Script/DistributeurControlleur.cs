using System.Collections; // N�cessaire pour IEnumerator
using UnityEngine;
using TMPro; // N�cessaire si tu utilises TextMeshPro


public class DistributeurController : MonoBehaviour
{
    public TextMeshProUGUI instructionText; // R�f�rence au texte d'instruction
    public ParticleSystem coffeeParticles; // Particules de caf�
    public Animator coffeeAnimator; // Animator pour l'animation de la machine
    public float fillTime = 5f; // Temps de remplissage en secondes
    private bool isFilling = false;

    void Start()
    {
        // Affiche le message initial
        instructionText.text = "Appuyez sur A pour obtenir du caf�.";
    }

    void Update()
    {
        // D�tecte l'appui sur le bouton A du joystick
        if (OVRInput.GetDown(OVRInput.Button.One) && !isFilling) // Bouton "A" par d�faut
        {
            StartCoroutine(FillCoffee());
        }
    }

    private IEnumerator FillCoffee()
    {
        isFilling = true;

        // Modifier le texte d'instruction
        instructionText.text = "Remplissage en cours...";

        // D�marre les particules de caf�
        coffeeParticles.Play();

        // Lancer l'animation de la machine
        if (coffeeAnimator != null)
        {
            coffeeAnimator.SetTrigger("ServeCoffee");
        }

        // Attendre le temps de remplissage (5 secondes)
        yield return new WaitForSeconds(fillTime);

        // Arr�ter les particules
        coffeeParticles.Stop();

        // Afficher le message de succ�s
        instructionText.text = "Votre caf� est pr�t !";

        // R�initialiser apr�s quelques secondes
        yield return new WaitForSeconds(3f);
        instructionText.text = "Appuyez sur A pour obtenir du caf�.";

        isFilling = false;
    }
}
