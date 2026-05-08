using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();



    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective objective)
    {
        if(!questProgress.ContainsKey(questSO))
            questProgress[questSO] = new Dictionary<QuestObjective, int>();
        
        var progressDictionary = questProgress[questSO];

        int newAmount = 0;

        if(objective.targetItem != null)
        newAmount = InventoryManager.Instance.GetItemQuantity(objective.targetItem);

        else if(objective.targetLocation != null && GameManager.instance.locationHistoryTracker.HasVisitedLocation(objective.targetLocation))
            newAmount = objective.requiredAmount;

        else if(objective.targetNPC != null && GameManager.instance.dialogueHistoryTracker.HasSpokenWithNPC(objective.targetNPC))
            newAmount = objective.requiredAmount;

        progressDictionary[objective] = newAmount;
    }

    public string GetProgressText(QuestSO questSO, QuestObjective objective)
    {
        int currentAmount = getCurrentAmount(questSO, objective);

        if(currentAmount >= objective.requiredAmount)
            return "Completed";
        else if(objective.targetItem != null)
            return $"{currentAmount}/{objective.requiredAmount}";
        return "In Progress";
    }

    public int getCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
           if(objectiveDictionary.TryGetValue(objective, out int amount))
             return amount;
        return 0;
    }
}
