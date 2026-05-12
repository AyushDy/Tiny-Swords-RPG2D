using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager2 : MonoBehaviour
{
    public static QuestManager2 Instance;

    private List<QuestRuntime> activeQuests = new List<QuestRuntime>();
    public IReadOnlyList<QuestRuntime> ActiveQuests => activeQuests;


    private List<QuestRuntime> completedQuests = new List<QuestRuntime>();


    private Dictionary<string, QuestRuntime> questLookup = new();

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        QuestEvents.onQuestOfferRequested += OnQuestOfferRequested;
    }

    private void OnDisable()
    {
        QuestEvents.onQuestOfferRequested -= OnQuestOfferRequested;
    }

    public void StartQuest(QuestSO2 questDefinition)
    {
        if (questDefinition == null)
        {
            Debug.LogError($"[QuestManager] Cannot start quest because the definition is null.");
            return;
        }

        if (questLookup.ContainsKey(questDefinition.questId))
        {
            Debug.LogError($"[QuestManager] Cannot start quest {questDefinition.questId} because it is already active.");
            return;
        }

        QuestRuntime runtimeQuest = QuestFactory.CreateRuntime(questDefinition);

        questLookup.Add(questDefinition.questId, runtimeQuest);

        activeQuests.Add(runtimeQuest);

        runtimeQuest.OnQuestReadyToTurnIn += OnQuestReadyToTurnIn;
        runtimeQuest.OnQuestCompleted += OnQuestCompleted;

        runtimeQuest.Accept();

        Debug.Log($"[QuestManager] Started quest : {questDefinition.questId}");
    }

    public QuestRuntime GetQuest(string questId)
    {
        if (questLookup.TryGetValue(questId, out QuestRuntime quest))
        {
            return quest;
        }
        return null;
    }

    private void OnQuestOfferRequested(QuestSO2 questSO)
    {
        if (questSO == null)
        {
            Debug.LogError($"[QuestManager] Cannot start quest because the definition is null.");
            return;
        }

        QuestSO2 convertedQuest = questSO as QuestSO2;

        if (convertedQuest == null)
        {
            Debug.LogError($"[QuestManager] Cannot start quest because the definition is not of type QuestSO2.");
            return;
        }

        StartQuest(convertedQuest);
    }

    private void OnQuestReadyToTurnIn(QuestRuntime quest)
    {
        Debug.Log($"[QuestManager] Quest ready to turn in : {quest.questId}");
    }

    private void OnQuestCompleted(QuestRuntime quest)
    {
        Debug.Log($"[QuestManager] Quest completed : {quest.questId}");

        activeQuests.Remove(quest);
        completedQuests.Add(quest);
    }

    public bool TryCompleteQuest(string questId)
    {
        QuestRuntime quest = GetQuest(questId);

        if (quest == null)
        {
            Debug.LogError($"[QuestManager] Quest with ID {questId} not found.");
            return false;
        }

        if (quest.State != QuestState.ReadyToTurnIn)
        {
            Debug.LogError($"[QuestManager] Quest with ID {questId} is not ready to turn in.");
            return false;
        }

        quest.Complete();
        return true;
    }

    public bool HasQuest(string questId)
    {
        return questLookup.ContainsKey(questId);
    }

    public bool IsQuestActive(string questId)
    {
        QuestRuntime quest = GetQuest(questId);
        if (quest == null)
        {
            return false;
        }
        return quest.State == QuestState.Accepted;
    }

    public bool IsQuestReadyToTurnIn(string questId)
    {
        QuestRuntime quest = GetQuest(questId);

        if (quest == null)
            return false;

        return quest.State == QuestState.ReadyToTurnIn;
    }

    public bool IsQuestCompleted(string questId)
    {
        QuestRuntime quest = GetQuest(questId);

        if (quest == null)
            return false;

        return quest.State == QuestState.Completed;
    }

    public bool TryTurnInQuest(string questId)
    {
        QuestRuntime quest = GetQuest(questId);

        if (quest == null)
        {
            Debug.LogError($"[QuestManager] Quest with ID {questId} not found.");
            return false;
        }

        for (int i = 0; i < quest.Objectives.Count; i++)
        {
            ObjectiveRuntime objective = quest.Objectives[i];

            SubmitItemObjectiveRuntime submitItemObjective = objective as SubmitItemObjectiveRuntime;

            if (submitItemObjective == null)
            {
                continue;
            }

            bool submitted = submitItemObjective.TrySubmit();

            if (!submitted)
            {
                Debug.LogError($"[QuestManager] Failed to submit item for objective {i} of quest {questId}.");
                return false;
            }


        }
        if (quest.State == QuestState.ReadyToTurnIn)
        {
            return TryCompleteQuest(questId);
        }


        return false;
    }
}
