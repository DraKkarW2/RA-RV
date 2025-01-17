using UnityEngine;

public class NPC : Entity
{
    public string Role; 

    public void Speak(string text)
    {
        Debug.Log($"{name} says: {text}");
    }

    public void Interact(Player player)
    {
        Debug.Log($"{name} is interacting with the player.");
    }

    public void GiveQuest(Quest quest)
    {
        if (quest != null && quest.CurrentState == QuestState.NotStarted)
        {
            quest.CurrentState = QuestState.InProgress;
            Debug.Log($"{name} gave the player a quest: {quest.Name}");
            // Ajoutez la quête à l'inventaire ou au gestionnaire de quêtes du joueur
        }
        else
        {
            Debug.Log($"{name} has no quests to give or the quest is already active.");
        }
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
