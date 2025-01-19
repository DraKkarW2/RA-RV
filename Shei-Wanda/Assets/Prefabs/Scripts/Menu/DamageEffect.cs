using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private Image damageImage;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float maxAlpha = 0.5f;

    private bool isFading;
    private float fadeTimer;

    private void Start()
    {
        if (damageImage != null)
        {
            damageImage.color = new Color(1, 0, 0, 0); // Rend l'image invisible au début
        }
    }

    public void TriggerDamageEffect()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true); // Assure l'activation du HUD
        }

        if (damageImage != null)
        {
            damageImage.color = new Color(1, 0, 0, maxAlpha);
            isFading = true;
            fadeTimer = fadeDuration;
        }
    }

    private void Update()
    {
        if (isFading && damageImage != null)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = Mathf.Lerp(0, maxAlpha, fadeTimer / fadeDuration);
            damageImage.color = new Color(1, 0, 0, alpha);

            if (fadeTimer <= 0)
            {
                isFading = false;
                damageImage.color = new Color(1, 0, 0, 0); // Complètement transparent
            }
        }
    }
}
