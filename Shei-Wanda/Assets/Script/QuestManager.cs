using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // Singleton pour un accès global
    public int totalPCsToClose = 10;  // Objectif de la quête
    private int closedPCsCount = 0;  // Nombre de PC fermés
    public TextMeshProUGUI questText;  // UI pour afficher la progression

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateQuestText();
    }

    public void ClosePC()
    {
        closedPCsCount++;

        if (closedPCsCount > totalPCsToClose)
            closedPCsCount = totalPCsToClose;

        UpdateQuestText();

        if (closedPCsCount >= totalPCsToClose)
        {
            QuestCompleted();
        }
    }

    void UpdateQuestText()
    {
        if (questText != null)
        {
            questText.text = $"PC fermés : {closedPCsCount}/{totalPCsToClose}";
        }
    }

    void QuestCompleted()
    {
        Debug.Log("Quête terminée ! Tous les PC ont été fermés.");
        questText.text = "Mission accomplie : 10/10 PC fermés";
    }
}
