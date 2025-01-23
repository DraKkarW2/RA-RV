using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance; // Singleton

    [Header("PC Quest")]
    public int totalPCsToClose = 1;
    private int closedPCsCount = 0;

    [Header("Presence Sheet Quest")]
    public int totalPresenceSheets = 1;
    private int collectedSheetsCount = 0;

    [Header("Bed Quest")]
    public int totalBedsToSleep = 1;
    private int sleptInBedsCount = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI pcQuestText;
    public TextMeshProUGUI presenceSheetQuestText;
    public TextMeshProUGUI bedQuestText;

    [Header("Item déjà dans la scène (désactivé)")]
    public GameObject itemInScene;     // ← glisse ici l'item depuis ta scène
    public Transform spawnPoint;       // ← point où l'item sera placé

    private bool itemEnabled = false;  // Pour éviter de le réactiver plusieurs fois

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // S'assurer que l'item est désactivé au départ
        if (itemInScene != null)
        {
            itemInScene.SetActive(false);
        }

        UpdateQuestText();
        UpdatePresenceSheetQuestText();
        UpdateBedQuestText();
    }

    // --- Quête PC ---
    public void ClosePC()
    {
        closedPCsCount++;
        if (closedPCsCount > totalPCsToClose)
            closedPCsCount = totalPCsToClose;

        UpdateQuestText();

        if (IsPCQuestCompleted())
        {
            PCQuestCompleted();
        }
    }

    public void UpdateQuestText()
    {
        if (pcQuestText != null)
        {
            pcQuestText.text = $"PC fermés : {closedPCsCount}/{totalPCsToClose}";
        }
    }

    private void PCQuestCompleted()
    {
        Debug.Log("Quête des PC terminée !");
        if (pcQuestText != null)
        {
            pcQuestText.text = $"Mission accomplie : {totalPCsToClose}/{totalPCsToClose} PC fermés";
        }

        CheckPCAndSheetsCompletion();
    }

    // --- Quête Feuilles de présence ---
    public void CollectPresenceSheet()
    {
        collectedSheetsCount++;
        if (collectedSheetsCount > totalPresenceSheets)
            collectedSheetsCount = totalPresenceSheets;

        UpdatePresenceSheetQuestText();

        if (IsPresenceSheetQuestCompleted())
        {
            PresenceSheetQuestCompleted();
        }
    }

    public void UpdatePresenceSheetQuestText()
    {
        if (presenceSheetQuestText != null)
        {
            presenceSheetQuestText.text = $"Feuilles de présence : {collectedSheetsCount}/{totalPresenceSheets}";
        }
    }

    private void PresenceSheetQuestCompleted()
    {
        Debug.Log("Quête des feuilles de présence terminée !");
        if (presenceSheetQuestText != null)
        {
            presenceSheetQuestText.text =
               $"Mission accomplie : {totalPresenceSheets}/{totalPresenceSheets} Feuilles signées";
        }

        CheckPCAndSheetsCompletion();
    }

    // --- Quête Lit ---
    public void SleepInBed()
    {
        sleptInBedsCount++;
        if (sleptInBedsCount > totalBedsToSleep)
            sleptInBedsCount = totalBedsToSleep;

        UpdateBedQuestText();

        if (IsBedQuestCompleted())
        {
            BedQuestCompleted();
        }
    }

    public void UpdateBedQuestText()
    {
        if (bedQuestText != null)
        {
            bedQuestText.text = $"Lit : {sleptInBedsCount}/{totalBedsToSleep}";
        }
    }

    private void BedQuestCompleted()
    {
        Debug.Log("Quête du lit terminée !");
        if (bedQuestText != null)
        {
            bedQuestText.text =
               $"Mission accomplie : {sleptInBedsCount}/{totalBedsToSleep}";
        }
    }

    // --- Check si PC et Feuilles terminées => on active l'item ---
    private void CheckPCAndSheetsCompletion()
    {
        if (IsPCQuestCompleted() && IsPresenceSheetQuestCompleted())
        {
            // Active l'item si pas déjà fait
            if (!itemEnabled)
            {
                SpawnItemInScene();
                itemEnabled = true;
            }
        }
    }

    // --- Au lieu d'un 'Instantiate', on replace l'item de la scène et on l'active ---
    private void SpawnItemInScene()
    {
        if (itemInScene == null || spawnPoint == null)
        {
            Debug.LogWarning("Impossible de rendre l'item actif : référence manquante.");
            return;
        }

        // On positionne l'item où on veut
        itemInScene.transform.position = spawnPoint.position;
        itemInScene.transform.rotation = spawnPoint.rotation;

        // On l'active
        itemInScene.SetActive(true);
        Debug.Log("Item activé dans la scène !");
    }

    // --- Méthodes publiques de vérif. ---
    public bool IsPCQuestCompleted()
    {
        return (closedPCsCount >= totalPCsToClose);
    }

    public bool IsPresenceSheetQuestCompleted()
    {
        return (collectedSheetsCount >= totalPresenceSheets);
    }

    public bool IsBedQuestCompleted()
    {
        return (sleptInBedsCount >= totalBedsToSleep);
    }

    public bool AreAllQuestsCompleted()
    {
        return (IsPCQuestCompleted() &&
                IsPresenceSheetQuestCompleted() &&
                IsBedQuestCompleted());
    }
}
