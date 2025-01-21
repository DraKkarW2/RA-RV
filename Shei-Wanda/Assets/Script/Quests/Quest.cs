using UnityEngine;

public class Quest : MonoBehaviour
{
  
    public string Name;
    public bool Requirement; 
    public Item Reward; 


    public QuestState CurrentState;


    public bool CheckRequirement()
    {
        
        if (Requirement)
        {
            Debug.Log($"Requirement for quest '{Name}' met.");
            return true;
        }
        Debug.Log($"Requirement for quest '{Name}' not met.");
        return false;
    }

    public void GiveReward()
    {
        if (CurrentState == QuestState.Completed)
        {
            Debug.Log($"Reward for quest '{Name}' already given.");
            return;
        }

        if (CheckRequirement())
        {
            CurrentState = QuestState.Completed;

            if (Reward != null)
            {
                Debug.Log($"Quest '{Name}' completed! Reward: {Reward.Name}.");
            }
            else
            {
                Debug.Log($"Quest '{Name}' completed! No reward assigned.");
            }
        }
        else
        {
            Debug.Log($"Quest '{Name}' not completed. Requirement not met.");
        }
    }
}
