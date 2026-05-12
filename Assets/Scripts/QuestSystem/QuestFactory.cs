using System;
using UnityEngine;

public static class QuestFactory
{
    public static QuestRuntime CreateRuntime(QuestSO2 questDefinition)
    {
        QuestRuntime runtimeQuest = new QuestRuntime(questDefinition);

        for(int i=0; i<questDefinition.objectives.Count; i++)
        {
            QuestObjectiveDefinition objective = questDefinition.objectives[i];

            ObjectiveRuntime runtimeObjective = CreateObjective(objective);

            if(runtimeObjective == null)
            {
                Debug.LogError($"[QuestFactory] Failed to create runtime objective for quest {questDefinition.questId} at index {i}");
                continue;
            }
            runtimeQuest.AddObjective(runtimeObjective);
        }
        return runtimeQuest;
    }

    public static ObjectiveRuntime CreateObjective(QuestObjectiveDefinition definition)
    {
        switch(definition.objectiveType)
        {
            case QuestObjectiveType.KillEnemy:
                return new KillEnemyObjectiveRuntime(definition.targetId, definition.requiredAmount);
            case QuestObjectiveType.Dialogue:
                return new DialogueObjectiveRuntime(definition.targetId);
            case QuestObjectiveType.CollectItem:
                return new CollectItemObjectiveRuntime(definition.targetItem, definition.requiredAmount);
            default:
                Debug.LogError($"[QuestFactory] Unsupported objective type {definition.objectiveType}");
                return null;
        }
    }


}
