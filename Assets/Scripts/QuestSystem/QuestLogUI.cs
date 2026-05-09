using System.Collections.Generic;
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


    [SerializeField] private QuestSO noAvailableQuestSO;
    [SerializeField] private QuestSlot[] questSlots;

    [SerializeField] private CanvasGroup questCanvas;
    [SerializeField] private CanvasGroup acceptCanvas;
    [SerializeField] private CanvasGroup DeclineCanvas;
    [SerializeField] private CanvasGroup completeCanvas;



    private void OnEnable()
    {
        QuestEvents.onQuestOfferRequested += ShowQuestOffer;
        QuestEvents.onQuestTurnInRequested += ShowQuestTurnIn;
    }

    private void OnDisable()
    {
        QuestEvents.onQuestOfferRequested -= ShowQuestOffer;
        QuestEvents.onQuestTurnInRequested -= ShowQuestTurnIn;
    }

    #region Show Quest Methods
    public void ShowQuestOffer(QuestSO incomingQuest)
    {
        if (questManager.isQuestAccepted(incomingQuest) || questManager.IsFinishedQuest(incomingQuest))
        {
            questSO = noAvailableQuestSO;

            SetCanvasGroup(acceptCanvas, false);
            SetCanvasGroup(DeclineCanvas, false);
            SetCanvasGroup(completeCanvas, false);

        }
        else
        {
            questSO = incomingQuest;

            SetCanvasGroup(acceptCanvas, true);
            SetCanvasGroup(DeclineCanvas, true);
            SetCanvasGroup(completeCanvas, false);

        }
        HandleQuestClicked(questSO);
        SetCanvasGroup(questCanvas, true);
    }

    public void ShowQuestTurnIn(QuestSO incomingQuest)
    {
        questSO = incomingQuest;

        HandleQuestClicked(incomingQuest);
        SetCanvasGroup(questCanvas, true);
        SetCanvasGroup(acceptCanvas, false);
        SetCanvasGroup(DeclineCanvas, false);
        SetCanvasGroup(completeCanvas, true);
    }



    #endregion



    #region On Button Clicked Methods
    public void OnAcceptQuestClicked()
    {
        questManager.AcceptQuest(questSO);
        SetCanvasGroup(completeCanvas, false);
        SetCanvasGroup(acceptCanvas, false);
        SetCanvasGroup(DeclineCanvas, false);
        RefreshQuestList();
        HandleQuestClicked(noAvailableQuestSO);
    }

    public void OnDeclineQuestClicked()
    {
        SetCanvasGroup(questCanvas, false);
    }

    public void OnCompleteQuestClicked()
    {
        questManager.CompleteQuest(questSO);
        RefreshQuestList();
        HandleQuestClicked(noAvailableQuestSO);

        SetCanvasGroup(completeCanvas, false);
    }

    #endregion

    private void SetCanvasGroup(CanvasGroup canvasGroup, bool activate)
    {
        canvasGroup.alpha = activate ? 1f : 0f;
        canvasGroup.interactable = activate;
        canvasGroup.blocksRaycasts = activate;
    }

    public void RefreshQuestList()
    {
        List<QuestSO> activeQuests = questManager.GetActiveQuests();

        for (int i = 0; i < questSlots.Length; i++)
        {
            if (i < activeQuests.Count)
            {
                questSlots[i].SetQuest(activeQuests[i]);
            }
            else
            {
                questSlots[i].ClearQuest();
            }
        }
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
