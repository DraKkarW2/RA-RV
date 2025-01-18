using UnityEngine;

public class HorrorFlickeringLight : MonoBehaviour
{
    public Light lightSource; // La lumi�re � contr�ler
    public float minIntensity = 0.2f; // Intensit� minimale
    public float maxIntensity = 2.0f; // Intensit� maximale
    public float flickerSpeed = 0.1f; // Vitesse du clignotement
    public bool enableFlickerSound = true; // Activer/d�sactiver le son de clignotement
    public AudioSource audioSource; // Source audio pour le son
    public AudioClip flickerSound; // Son de clignotement

    private float targetIntensity;
    private float timer;

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }
        targetIntensity = lightSource.intensity;
    }

    void Update()
    {
        // G�rer le clignotement
        timer += Time.deltaTime;
        if (timer >= flickerSpeed)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = 0;

            // Jouer un son de clignotement si activ�
            if (enableFlickerSound && audioSource != null && flickerSound != null)
            {
                audioSource.PlayOneShot(flickerSound);
            }
        }

        // Doucement passer � la nouvelle intensit�
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, Time.deltaTime * 10);
    }
}
