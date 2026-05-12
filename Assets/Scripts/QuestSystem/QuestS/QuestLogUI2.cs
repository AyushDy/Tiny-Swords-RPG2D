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


    private QuestRuntime selectedQuest;

    private void OnEnable()
    {
        RefreshQuestList();
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
        selectedQuest = runtimeQuest;

        RefreshQuestDetails();
    }

    private void RefreshQuestDetails()
    {
        if(selectedQuest == null ) return;

        questTitleText.text = selectedQuest.questDefinition.questName;
        questDescriptionText.text = selectedQuest.questDefinition.questDescription;

        RefreshObjectives();
        RefreshRewards();
    }

    private void RefreshObjectives()
    {
        for (int i=0; i < objectiveSlots.Length; i++)
        {
            if (i < selectedQuest.Objectives.Count)
            {
                ObjectiveRuntime objective = selectedQuest.Objectives[i];

                objectiveSlots[i].gameObject.SetActive(true);

                objectiveSlots[i].RefreshObjectives(
                    objective.Description,
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
}
