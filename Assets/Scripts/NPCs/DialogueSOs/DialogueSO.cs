using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Runtime")]
    public string dialogueId;

    [Header("Offered Quest (Optional)")]
    public QuestSO2 offerQuestOnEnd;

    [Header("Turn in Quest (Optional)")]
    public QuestSO2 turnInQuestOnEnd;

    [Header("Conditional Requirements (Optional)")]
    public ActorSO[] requiredNPCs;
    public LocationSO[] requiredLocations;
    public ItemSO[] requiredItems;

    [Header("Quest State Requirements")]
    public QuestSO2 requiredQuest;
    public QuestDialogueState requiredQuestState;

    [Header("Control Flags")]
    public bool RemoveAfterPlay;
    public List<DialogueSO> removeTheseOnPlay;


    public bool isConditionMet()
    {
        if(requiredNPCs.Length > 0)
        {
            foreach(var npc in requiredNPCs)
            {
                if(!GameManager.instance.dialogueHistoryTracker.HasSpokenWithNPC(npc))
                    return false;
            }
        }

        if(requiredLocations.Length > 0)
        {
            foreach(var location in requiredLocations)
            {
                if(!GameManager.instance.locationHistoryTracker.HasVisitedLocation(location))
                    return false;
            }
        }

        if(requiredItems.Length > 0)
        {
            foreach(var item in requiredItems)
            {
                if(!InventoryManager.Instance.HasItem(item))
                    return false;
            }
        }

        if(requiredQuest != null)
        {
            string questId = requiredQuest.questId;

            switch (requiredQuestState)
            {
                case QuestDialogueState.Available :
                    if(!QuestManager2.Instance.IsQuestAvailable(questId))
                        return false;
                    break;
                
                case QuestDialogueState.Active :
                    if(!QuestManager2.Instance.IsQuestActive(questId))
                        return false;
                    break;
                
                case QuestDialogueState.ReadyToTurnIn :
                    if(!QuestManager2.Instance.IsQuestReadyToTurnIn(questId))
                        return false;
                    break;
                
                case QuestDialogueState.Completed :
                    if(!QuestManager2.Instance.IsQuestCompleted(questId))
                        return false;
                    break;
            }
        }

        return true;
    }
}


[System.Serializable]
public class DialogueLine
{
    public ActorSO speaker;
    [TextArea(3, 10)] public string text;
}

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueSO nextDialogue;
    public QuestSO2 offerQuest;
}

public enum QuestDialogueState
{
    None,
    Available,
    Active,
    ReadyToTurnIn,
    Completed,
}