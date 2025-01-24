using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public Player player;  // Référence au script Player
    public Image santéBar;  // Référence à la barre de santé (Image)
    public TextMeshProUGUI santéText;  // Référence au texte de la santé

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();  // Trouver automatiquement le joueur dans la scène
        }

        if (player != null)
        {
            // S'abonner à l'événement de changement de santé
            player.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI(player.Health);  // Initialiser l'UI avec la santé actuelle
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(int currentHealth)
    {
        float fillAmount = (float)currentHealth / 100f;
        santéBar.fillAmount = fillAmount;  // Mise à jour de la barre

        santéText.text = $"Santé : {currentHealth} / 100";  // Mise à jour du texte
    }
}
