using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // Singleton pour un acc�s global
    public int totalPCsToClose = 10;  // Objectif de la qu�te
    private int closedPCsCount = 0;  // Nombre de PC ferm�s
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
            questText.text = $"PC ferm�s : {closedPCsCount}/{totalPCsToClose}";
        }
    }

    void QuestCompleted()
    {
        Debug.Log("Qu�te termin�e ! Tous les PC ont �t� ferm�s.");
        questText.text = "Mission accomplie : 10/10 PC ferm�s";
    }
}
