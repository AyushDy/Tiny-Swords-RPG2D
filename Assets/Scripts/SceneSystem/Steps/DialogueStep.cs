using System.Collections;
using UnityEngine;


[System.Serializable]
public class DialogueStep : SceneStep
{
    public DialogueSO dialogue;
    
    public override IEnumerator Execute(SceneDirector director)
    {
        if(dialogue == null)
        {
            yield break;
        }

        director.dialogueManager.StartDialogue(dialogue);

        yield return new WaitUntil(() => 
            !director.dialogueManager.isDialogueActive
        );
    }
}
