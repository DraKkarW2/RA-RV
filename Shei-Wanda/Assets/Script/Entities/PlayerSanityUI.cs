using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSanityUI : MonoBehaviour
{
    public Player player;
    public Image sanitéBar;
    public TextMeshProUGUI sanitéText;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        if (player != null)
        {
            player.OnSanityChanged += UpdateSanityUI;
            UpdateSanityUI(player.Sanity);
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnSanityChanged -= UpdateSanityUI;
        }
    }

    private void UpdateSanityUI(int currentSanity)
    {
        float fillAmount = (float)currentSanity / 100f;
        sanitéBar.fillAmount = fillAmount;
        sanitéText.text = $"Sanité : {currentSanity} / 100";
    }
}
