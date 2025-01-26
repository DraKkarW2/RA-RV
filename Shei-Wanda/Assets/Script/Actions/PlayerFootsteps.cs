using UnityEngine;

/// <summary>
/// Script qui joue un son de marche et un son de sprint, chacun sur une AudioSource distincte.
/// Se base sur la vitesse de déplacement et la variable Sprint du script Player.
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("AudioSource dédiée au son de marche (loop = true)")]
    public AudioSource walkAudioSource;

    [Tooltip("AudioSource dédiée au son de sprint (loop = true)")]
    public AudioSource runAudioSource;

    [Header("Seuil de vitesse")]
    [Tooltip("Vitesse à partir de laquelle on considère qu'on se déplace (m/s)")]
    public float walkThreshold = 0.1f;

    // Référence au script Player (pour accéder à 'Sprint')
    private Player player;

    // Pour calculer la vitesse : on se souvient de la dernière position
    private Vector3 lastPosition;

    private void Start()
    {
        // Récupère le composant Player sur le même objet
        player = GetComponent<Player>();

        // Initialise la variable pour la comparaison de positions
        lastPosition = transform.position;

        // Assure-toi que tes AudioSource soient configurées pour boucler (loop)
        if (walkAudioSource != null)
        {
            walkAudioSource.loop = true;
        }
        if (runAudioSource != null)
        {
            runAudioSource.loop = true;
        }
    }

    private void Update()
    {
        // Calcul de la vitesse du GameObject
        Vector3 currentPosition = transform.position;
        float distanceFrame = Vector3.Distance(currentPosition, lastPosition);
        float currentSpeed = distanceFrame / Time.deltaTime; // en m/s
        lastPosition = currentPosition;

        // Si la vitesse est supérieure au seuil, on joue un des deux sons
        if (currentSpeed > walkThreshold)
        {
            if (player.Sprint)
            {
                // Lecture du son de sprint
                if (runAudioSource != null && !runAudioSource.isPlaying)
                {
                    runAudioSource.Play();
                }
                // On arrête la marche si elle joue
                if (walkAudioSource != null && walkAudioSource.isPlaying)
                {
                    walkAudioSource.Stop();
                }
            }
            else
            {
                // Lecture du son de marche
                if (walkAudioSource != null && !walkAudioSource.isPlaying)
                {
                    walkAudioSource.Play();
                }
                // On arrête le sprint si nécessaire
                if (runAudioSource != null && runAudioSource.isPlaying)
                {
                    runAudioSource.Stop();
                }
            }
        }
        else
        {
            // Vitesse trop faible => on coupe tout
            if (walkAudioSource != null && walkAudioSource.isPlaying)
            {
                walkAudioSource.Stop();
            }
            if (runAudioSource != null && runAudioSource.isPlaying)
            {
                runAudioSource.Stop();
            }
        }
    }
}
