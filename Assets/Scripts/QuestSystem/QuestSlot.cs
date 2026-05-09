using TMPro;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;

    public QuestSO currentQuest;
    public QuestLogUI questLogUI;


    public void SetQuest(QuestSO quest)
    {
        currentQuest = quest;
        questNameText.text = quest.questName;
    }

    public void ClearQuest()
    {
        currentQuest = null;
        gameObject.SetActive(false);
    }

    public void OnQuestSlotClicked()
    {
        if (currentQuest != null)
        {
            questLogUI.HandleQuestClicked(currentQuest);
        }
    }
}
