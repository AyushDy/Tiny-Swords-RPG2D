using TMPro;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;

    public QuestSO currentQuest;
    public QuestLogUI questLogUI;


    private void OnValidate()
    {
        if (currentQuest != null)
            SetQuest(currentQuest);
    }


    public void SetQuest(QuestSO quest)
    {
        currentQuest = quest;
        questNameText.text = quest.questName;
    }

    public void OnQuestSlotClicked()
    {
        if (currentQuest != null)
        {
            questLogUI.HandleQuestClicked(currentQuest);
        }
    }
}
