using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusUI : MonoBehaviour
{
    public Image santéBar;
    public Image sanitéBar;
    public TextMeshProUGUI santéText;
    public TextMeshProUGUI sanitéText;

    private int maxValue = 100;

    public void UpdateSanté(int currentHealth)
    {
        float fillAmount = (float)currentHealth / maxValue;
        santéBar.fillAmount = fillAmount;
        santéText.text = "Santé : " + currentHealth + " / " + maxValue;
    }

    public void UpdateSanité(int currentSanity)
    {
        float fillAmount = (float)currentSanity / maxValue;
        sanitéBar.fillAmount = fillAmount;
        sanitéText.text = "Sanité : " + currentSanity + " / " + maxValue;
    }
}
