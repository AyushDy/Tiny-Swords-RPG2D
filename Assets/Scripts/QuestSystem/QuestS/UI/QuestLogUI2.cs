using TMPro;
using UnityEngine;

public class QuestLogUI2 : MonoBehaviour
{
    [Header("Quest Details")]
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;


    [Header("Objective UI")]
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;

    [Header("Reward UI")]
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    [Header("Quest List")]
    [SerializeField] private QuestLogQuestSlot[] questSlots;

    public CanvasGroup questLogCanvasGroup;

    private QuestRuntime selectedQuest;



    private void Start()
    {
        if (QuestManager2.Instance == null)
        {
            Debug.Log($"[QuestLogUI] No QuestManager instance found in the scene.");
            return;
        }
    }
    private void OnEnable()
    {
        if(QuestManager2.Instance == null)
        {
            Debug.Log($"[QuestLogUI] No QuestManager instance found in the scene.");
            return;
        }

        QuestManager2.Instance.OnQuestStarted += OnQuestListChanged;
        QuestManager2.Instance.OnQuestRemoved += OnQuestListChanged;
        
        RefreshQuestList();
    }

    private void OnDisable()
    {
        if (QuestManager2.Instance != null)
        {
            QuestManager2.Instance.OnQuestStarted -= OnQuestListChanged;
            QuestManager2.Instance.OnQuestRemoved -= OnQuestListChanged;
        }
    }

    public void RefreshQuestList()
    {
        var activeQuests = QuestManager2.Instance.ActiveQuests;

        for (int i = 0; i < questSlots.Length; i++)
        {
            if (i < activeQuests.Count)
            {
                questSlots[i].Initialize(activeQuests[i], this);
            }
            else
            {
                questSlots[i].Clear();
            }
        }

        if (selectedQuest == null && activeQuests.Count > 0)
        {
            SelectQuest(activeQuests[0]);
        }
    }

    public void SelectQuest(QuestRuntime runtimeQuest)
    {
        if (selectedQuest != null)
        {
            selectedQuest.OnQuestUpdated -= OnSelectedQuestUpdated;
        }

        selectedQuest = runtimeQuest;

        selectedQuest.OnQuestUpdated += OnSelectedQuestUpdated;

        RefreshQuestDetails();
    }

    private void OnSelectedQuestUpdated(QuestRuntime quest)
    {
        if (quest != selectedQuest) return;

        RefreshQuestDetails();
    }

    private void RefreshQuestDetails()
    {
        if (selectedQuest == null) return;

        questTitleText.text = selectedQuest.questDefinition.questName;
        questDescriptionText.text = selectedQuest.questDefinition.questDescription;

        Debug.Log($"[QuestLogUI] Refreshing UI for state: {selectedQuest.State}");

        RefreshObjectives();
        RefreshRewards();
    }

    private void RefreshObjectives()
    {
        for (int i = 0; i < objectiveSlots.Length; i++)
        {
            if (i < selectedQuest.Objectives.Count)
            {
                ObjectiveRuntime objective = selectedQuest.Objectives[i];

                objectiveSlots[i].gameObject.SetActive(true);

                objectiveSlots[i].RefreshObjectives(
                    objective.description,
                    $"{objective.CurrentAmount}/{objective.RequiredAmount}",
                    objective.State == ObjectiveState.Completed
                );
            }
            else
            {
                objectiveSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshRewards()
    {
        var rewards = selectedQuest.questDefinition.rewards;

        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < rewards.Count)
            {
                rewardSlots[i].gameObject.SetActive(true);

                rewardSlots[i].DisplayReward(
                    rewards[i].itemSO.icon,
                    rewards[i].quantity
                );
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnQuestListChanged(QuestRuntime quest)
    {
        Debug.Log("[QuestLogUI] Received quest list change");
        RefreshQuestList();
    }

    public void CloseQuestCanvas()
    {
        questLogCanvasGroup.alpha = 0;
        questLogCanvasGroup.interactable = false;
        questLogCanvasGroup.blocksRaycasts = false;
    }
}
