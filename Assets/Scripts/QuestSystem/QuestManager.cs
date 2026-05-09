using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();
    private List<QuestSO> finishedQuests = new List<QuestSO>();



    private void OnEnable()
    {
        QuestEvents.IsQuestComplete += IsQuestComplete;
    }

    private void OnDisable()
    {
        QuestEvents.IsQuestComplete -= IsQuestComplete;
    }

    public bool IsQuestComplete(QuestSO questSO)
    {
        if (!questProgress.TryGetValue(questSO, out var objectiveDictionary))
            return false;

        foreach (var objective in questSO.objectives)
            UpdateObjectiveProgress(questSO, objective);

        foreach (var objective in questSO.objectives)
            if (objectiveDictionary[objective] < objective.requiredAmount)
                return false;
        return true;
    }

    public bool IsFinishedQuest(QuestSO questSO)
    {
        return finishedQuests.Contains(questSO);
    }

    public void CompleteQuest(QuestSO questSO)
    {
        questProgress.Remove(questSO);
        finishedQuests.Add(questSO);

        foreach (var reward in questSO.rewards)
        {
            InventoryManager.Instance.AddToInventory(reward.itemSO, reward.quantity);
        }
    }


    public List<QuestSO> GetActiveQuests()
    {
        return new List<QuestSO>(questProgress.Keys);
    }

    public bool isQuestAccepted(QuestSO questSO)
    {
        return questProgress.ContainsKey(questSO);
    }


    public void AcceptQuest(QuestSO questSO)
    {
        questProgress[questSO] = new Dictionary<QuestObjective, int>();

        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
    }

    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective objective)
    {
        if (!questProgress.ContainsKey(questSO))
            return;

        var progressDictionary = questProgress[questSO];

        int newAmount = 0;

        if (objective.targetItem != null)
            newAmount = InventoryManager.Instance.GetItemQuantity(objective.targetItem);

        else if (objective.targetLocation != null && GameManager.instance.locationHistoryTracker.HasVisitedLocation(objective.targetLocation))
            newAmount = objective.requiredAmount;

        else if (objective.targetNPC != null && GameManager.instance.dialogueHistoryTracker.HasSpokenWithNPC(objective.targetNPC))
            newAmount = objective.requiredAmount;

        progressDictionary[objective] = newAmount;
    }

    public string GetProgressText(QuestSO questSO, QuestObjective objective)
    {
        int currentAmount = getCurrentAmount(questSO, objective);

        if (currentAmount >= objective.requiredAmount)
            return "Completed";
        else if (objective.targetItem != null)
            return $"{currentAmount}/{objective.requiredAmount}";
        return "In Progress";
    }

    public int getCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
            if (objectiveDictionary.TryGetValue(objective, out int amount))
                return amount;
        return 0;
    }
}
