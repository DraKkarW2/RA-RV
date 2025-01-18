using System.Collections; // Nécessaire pour IEnumerator
using UnityEngine;
using TMPro; // Nécessaire si tu utilises TextMeshPro


public class DistributeurController : MonoBehaviour
{
    public TextMeshProUGUI instructionText; // Référence au texte d'instruction
    public ParticleSystem coffeeParticles; // Particules de café
    public Animator coffeeAnimator; // Animator pour l'animation de la machine
    public float fillTime = 5f; // Temps de remplissage en secondes
    private bool isFilling = false;

    void Start()
    {
        // Affiche le message initial
        instructionText.text = "Appuyez sur A pour obtenir du café.";
    }

    void Update()
    {
        // Détecte l'appui sur le bouton A du joystick
        if (OVRInput.GetDown(OVRInput.Button.One) && !isFilling) // Bouton "A" par défaut
        {
            StartCoroutine(FillCoffee());
        }
    }

    private IEnumerator FillCoffee()
    {
        isFilling = true;

        // Modifier le texte d'instruction
        instructionText.text = "Remplissage en cours...";

        // Démarre les particules de café
        coffeeParticles.Play();

        // Lancer l'animation de la machine
        if (coffeeAnimator != null)
        {
            coffeeAnimator.SetTrigger("ServeCoffee");
        }

        // Attendre le temps de remplissage (5 secondes)
        yield return new WaitForSeconds(fillTime);

        // Arrêter les particules
        coffeeParticles.Stop();

        // Afficher le message de succès
        instructionText.text = "Votre café est prêt !";

        // Réinitialiser après quelques secondes
        yield return new WaitForSeconds(3f);
        instructionText.text = "Appuyez sur A pour obtenir du café.";

        isFilling = false;
    }
}
