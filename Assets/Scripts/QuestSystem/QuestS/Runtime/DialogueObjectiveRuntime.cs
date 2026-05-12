using UnityEngine;

public class DialogueObjectiveRuntime : ObjectiveRuntime
{
    private string dialogueId;

    public DialogueObjectiveRuntime(string dialogueId)
    {
        this.dialogueId = dialogueId;
    }

    public override void Activate()
    {
        State = ObjectiveState.Active;

        EventBus.Subscribe<DialogueFinishedEvent>(
            dialogueId,
            OnDialogueFinished
        );

        Debug.Log($"[Objective] listening for dialogue : {dialogueId}");
    }

    public override void Deactivate()
    {
        EventBus.Unsubscribe<DialogueFinishedEvent>(
            dialogueId,
            OnDialogueFinished
        );
    }

    private void OnDialogueFinished(object data)
    {
        if (State == ObjectiveState.Completed)
        {
            return;
        }

        Complete();
    }

    private void Complete()
    {
        State = ObjectiveState.Completed;

        Debug.Log($"[Objective] Dialogue {dialogueId} completed!");

        NotifyCompleted();
        Deactivate();
    }
}
