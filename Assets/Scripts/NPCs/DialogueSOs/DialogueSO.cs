using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Requirements (Optional)")]
    public ActorSO[] requiredNPCs;
    public LocationSO[] requiredLocations;
    public ItemSO[] requiredItems;

    [Header("Control Flags")]
    public bool RemoveAfterPlay;
    public List<DialogueSO> removeTheseOnPlay;


    public bool isConditionMet()
    {
        if(requiredNPCs.Length > 0)
        {
            foreach(var npc in requiredNPCs)
            {
                if(!DialogueHistoryTracker.Instance.HasSpokenWithNPC(npc))
                    return false;
            }
        }
        if(requiredLocations.Length > 0)
        {
            foreach(var location in requiredLocations)
            {
                if(!LocationHistoryTracker.Instance.HasVisitedLocation(location))
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
}