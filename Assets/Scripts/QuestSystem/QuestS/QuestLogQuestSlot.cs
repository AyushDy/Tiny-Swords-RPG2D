using TMPro;
using UnityEngine;

public class QuestLogQuestSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;

    private QuestRuntime runtimeQuest;

    private QuestLogUI2 questLogUI;

    public void Initialize(
        QuestRuntime runtimeQuest,
        QuestLogUI2 questLogUI
    )
    {
        this.runtimeQuest = runtimeQuest;
        this.questLogUI = questLogUI;

        questNameText.text = runtimeQuest.questDefinition.questName;

        gameObject.SetActive(true);
    }


    public void Clear()
    {
        runtimeQuest = null;
        gameObject.SetActive(false);
    }


    public void OnSlotClicked()
    {
        if(runtimeQuest == null) return;

        questLogUI.SelectQuest(runtimeQuest);
    }
}
