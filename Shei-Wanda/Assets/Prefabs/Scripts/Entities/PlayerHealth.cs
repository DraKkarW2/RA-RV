using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public Player player;  // Référence au script Player (à attribuer dans l'Inspector)
    public Image healthBarImage;  // Référence à la barre de santé
    public TextMeshProUGUI healthText;  // Référence au texte de santé

    private int lastHealth;  // Pour vérifier les changements

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerHealth: Référence au script Player non attribuée.");
            return;
        }

        // Initialisation de l'UI avec la santé actuelle
        lastHealth = player.Health;
        UpdateHealthUI();
    }

    void Update()
    {
        // Vérifie si la santé du joueur a changé
        if (player.Health != lastHealth)
        {
            lastHealth = player.Health;
            UpdateHealthUI();
        }
    }

    private void UpdateHealthUI()
    {
        float fillAmount = (float)player.Health / 100f;  // Mise à l'échelle entre 0 et 1
        healthBarImage.fillAmount = fillAmount;
        healthText.text = $"Santé : {player.Health}/100";
    }
}
