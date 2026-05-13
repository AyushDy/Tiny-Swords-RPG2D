using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestRuntime
{
    public string questId { get; private set; }
    public QuestState State { get; private set; }
    public QuestSO2 questDefinition { get; private set; }

    public Action<QuestRuntime> OnQuestReadyToTurnIn;
    public Action<QuestRuntime> OnQuestCompleted;
    public Action<QuestRuntime> OnQuestUpdated;

    private List<ObjectiveRuntime> objectives = new List<ObjectiveRuntime>();
    public IReadOnlyList<ObjectiveRuntime> Objectives => objectives;

    public QuestRuntime(QuestSO2 definition)
    {
        questDefinition = definition;
        State = QuestState.Available;
        questId = definition.questId;
    }

    public void AddObjective(ObjectiveRuntime objective)
    {
        objectives.Add(objective);

        objective.OnObjectiveCompleted += CheckCompletion;
        objective.OnObjectiveProgressed += NotifyQuestUpdated;
    }

    public void Accept()
    {
        if (State != QuestState.Available)
        {
            Debug.LogError($"[Quest] Cannot accept quest {questId} because it is not available.");
            return;
        }

        State = QuestState.Accepted;
        ActivateObjectives();
    }

    private void ActivateObjectives()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].Activate();
        }
    }

    public void CheckCompletion()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].State != ObjectiveState.Completed)
            {
                return;
            }
        }

        MarkReadyToTurnIn();
    }

    private void MarkReadyToTurnIn()
    {
        State = QuestState.ReadyToTurnIn;

        NotifyQuestUpdated();

        Debug.Log($"[Quest] Quest {questId} is ready to turn in!");

        OnQuestReadyToTurnIn?.Invoke(this);
    }

    public void Complete()
    {
        if (State != QuestState.ReadyToTurnIn)
        {
            Debug.LogError($"[Quest] Cannot complete quest {questId} because it is not ready to turn in.");
            return;
        }

        State = QuestState.Completed;

        NotifyQuestUpdated();

        Debug.Log($"[Quest] Quest {questId} completed!");

        OnQuestCompleted?.Invoke(this);
    }

    public void Fail()
    {
        if (State == QuestState.Completed)
            return;

        State = QuestState.Failed;

        NotifyQuestUpdated();

        Debug.Log($"[Quest] Quest {questId} failed.");

        CleanupObjectives();
    }

    public void Abandon()
    {
        if (State == QuestState.Completed)
            return;

        State = QuestState.Abandoned;

        NotifyQuestUpdated();

        Debug.Log($"[Quest] Quest {questId} abandoned.");

        CleanupObjectives();
    }


    private void CleanupObjectives()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].Deactivate();
        }
    }

    private void NotifyQuestUpdated()
    {
        OnQuestUpdated?.Invoke(this);
    }
}