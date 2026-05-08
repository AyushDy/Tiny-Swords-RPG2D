using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;

    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    private QuestSO questSO;

    [SerializeField] private CanvasGroup questCanvas;
    [SerializeField] private CanvasGroup acceptCanvas;
    [SerializeField] private CanvasGroup DeclineCanvas;
    [SerializeField] private CanvasGroup completeCanvas;



    private void OnEnable()
    {
        QuestEvents.onQuestOfferRequested += ShowQuestOffer;
    }

    private void OnDisable()
    {
        QuestEvents.onQuestOfferRequested -= ShowQuestOffer;
    }

    public void ShowQuestOffer(QuestSO incomingQuest)
    {
        HandleQuestClicked(incomingQuest);
        SetCanvasGroup(questCanvas, true);
        SetCanvasGroup(acceptCanvas, true);
        SetCanvasGroup(DeclineCanvas, true);
        SetCanvasGroup(completeCanvas, false);
    }

    private void SetCanvasGroup(CanvasGroup canvasGroup, bool activate)
    {
        canvasGroup.alpha = activate ? 1f : 0f;
        canvasGroup.interactable = activate;
        canvasGroup.blocksRaycasts = activate;
    }


    public void HandleQuestClicked(QuestSO questSO)
    {
        this.questSO = questSO;
        questNameText.text = questSO.questName;
        questDescriptionText.text = questSO.questDescription;

        DisplayObjectives();
        DisplayRewards();
        foreach (var objective in questSO.objectives)
        {
            questManager.UpdateObjectiveProgress(questSO, objective);
            Debug.Log($"Objective: {objective.description} => {questManager.GetProgressText(questSO, objective)}");
        }
    }


    private void DisplayObjectives()
    {
        for (int i = 0; i < objectiveSlots.Length; i++)
        {
            if (i < questSO.objectives.Count)
            {
                var objective = questSO.objectives[i];
                questManager.UpdateObjectiveProgress(questSO, objective);

                int currentAmount = questManager.getCurrentAmount(questSO, objective);
                string progressText = questManager.GetProgressText(questSO, objective);
                bool isComplete = currentAmount >= objective.requiredAmount;

                objectiveSlots[i].gameObject.SetActive(true);
                objectiveSlots[i].RefreshObjectives(objective.description, progressText, isComplete);
            }
            else
            {
                objectiveSlots[i].gameObject.SetActive(false);
            }
        }
    }


    private void DisplayRewards()
    {
        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < questSO.rewards.Count)
            {
                var reward = questSO.rewards[i];
                rewardSlots[i].DisplayReward(reward.itemSO.icon, reward.quantity);
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
