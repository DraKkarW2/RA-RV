using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSanity : MonoBehaviour
{
    public Image sanityBarImage;  // Référence à l'image de la barre de sanité
    public TextMeshProUGUI sanityText;  // Référence au texte de sanité

    private Player player;  // Référence au script Player

    private void Start()
    {
        // Trouve le script Player attaché au GameObject
        player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("PlayerSanity: Le script Player est introuvable sur l'objet.");
        }

        UpdateSanityUI();
    }

    public void ReduceSanity(int amount)
    {
        if (player != null)
        {
            player.Sanity -= amount;
            UpdateSanityUI();
        }
    }

    private void UpdateSanityUI()
    {
        float fillAmount = (float)player.Sanity / 100f;  // Mise à l'échelle entre 0 et 1
        sanityBarImage.fillAmount = fillAmount;
        sanityText.text = $"Sanité : {player.Sanity}/100";
    }
}
