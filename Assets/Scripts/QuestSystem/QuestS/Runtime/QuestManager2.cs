using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager2 : MonoBehaviour
{
    public static QuestManager2 Instance;

    public Action<QuestRuntime> OnQuestStarted;
    public Action<QuestRuntime> OnQuestRemoved;
    public Action<QuestSO2> OnQuestOfferReceived;

    private List<QuestRuntime> activeQuests = new List<QuestRuntime>();
    public IReadOnlyList<QuestRuntime> ActiveQuests => activeQuests;

    private List<QuestRuntime> completedQuests = new List<QuestRuntime>();

    private Dictionary<string, QuestRuntime> questLookup = new();

    private QuestSO2 pendingQuestOffer;

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        QuestEvents.onQuestOfferRequested += OnQuestOfferRequested;
        QuestEvents.onQuestTurnInRequested += OnQuestTurnInRequested;
    }

    private void OnDisable()
    {
        QuestEvents.onQuestOfferRequested -= OnQuestOfferRequested;
        QuestEvents.onQuestTurnInRequested -= OnQuestTurnInRequested;
    }

    public void StartQuest(QuestSO2 questDefinition)
    {
        if (questDefinition == null)
        {
            Debug.Log($"[QuestManager] Cannot start quest because the definition is null.");
            return;
        }

        if (questLookup.ContainsKey(questDefinition.questId))
        {
            Debug.Log($"[QuestManager] Cannot start quest {questDefinition.questId} because it is already active.");
            return;
        }

        QuestRuntime runtimeQuest = QuestFactory.CreateRuntime(questDefinition);

        questLookup.Add(questDefinition.questId, runtimeQuest);

        activeQuests.Add(runtimeQuest);

        runtimeQuest.OnQuestReadyToTurnIn += OnQuestReadyToTurnIn;
        runtimeQuest.OnQuestCompleted += OnQuestCompleted;
        runtimeQuest.OnQuestUpdated += OnQuestUpdated;

        runtimeQuest.Accept();

        Debug.Log("[QuestManager] Invoking OnQuestStarted");

        OnQuestStarted?.Invoke(runtimeQuest);

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
            Debug.Log($"[QuestManager] Cannot start quest because the definition is null.");
            return;
        }

        QuestSO2 convertedQuest = questSO as QuestSO2;

        if (convertedQuest == null)
        {
            Debug.Log($"[QuestManager] Cannot start quest because the definition is not of type QuestSO2.");
            return;
        }
        pendingQuestOffer = convertedQuest;

        OnQuestOfferReceived?.Invoke(pendingQuestOffer);

        Debug.Log($"[QuestOffer] Pending offer: {convertedQuest.questId}");
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

        OnQuestRemoved?.Invoke(quest);
    }

    private void OnQuestUpdated(QuestRuntime quest)
    {
        Debug.Log($"[QuestManager] Quest updated : {quest.questId}");
    }

    public bool TryCompleteQuest(string questId)
    {
        QuestRuntime quest = GetQuest(questId);

        if (quest == null)
        {
            Debug.Log($"[QuestManager] Quest with ID {questId} not found.");
            return false;
        }

        if (quest.State != QuestState.ReadyToTurnIn)
        {
            Debug.Log($"[QuestManager] Quest with ID {questId} is not ready to turn in.");
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
            Debug.Log($"[QuestManager] Quest with ID {questId} not found.");
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
                Debug.Log($"[QuestManager] Failed to submit item for objective {i} of quest {questId}.");
                return false;
            }


        }
        if (quest.State == QuestState.ReadyToTurnIn)
        {
            return TryCompleteQuest(questId);
        }


        return false;
    }

    public void AcceptPendingQuestOffer()
    {
        if(pendingQuestOffer == null)
        {
            Debug.Log($"[QuestManager] No pending quest offer to accept.");
            return;
        }

        StartQuest(pendingQuestOffer);
        pendingQuestOffer = null;
    }
   
    public void DeclinePendingQuestOffer()
    {
        if(pendingQuestOffer == null)
        {
            Debug.Log($"[QuestManager] No pending quest offer to decline.");
            return;
        }

        Debug.Log($"[QuestManager] Declined quest offer: {pendingQuestOffer.questId}");
        pendingQuestOffer = null;
    }

    public bool IsQuestAvailable(string questId)
    {
        return(!questLookup.ContainsKey(questId));
    }

    private void OnQuestTurnInRequested(QuestSO2 questSO)
    {
        if(questSO == null)
        {
            Debug.Log($"[QuestManager] Turn-in quest is null.");
            return;
        }

        TryTurnInQuest(questSO.questId);
    }


}
