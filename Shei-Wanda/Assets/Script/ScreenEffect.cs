using UnityEngine;
using TMPro;

public class ScreenEffect : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public float flickerSpeed = 0.5f;
    private float timer;
    private bool isVisible = true;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= flickerSpeed)
        {
            isVisible = !isVisible;
            questText.alpha = isVisible ? 1f : 0.8f;  // Simule un léger clignotement
            timer = 0;
        }
    }
}
