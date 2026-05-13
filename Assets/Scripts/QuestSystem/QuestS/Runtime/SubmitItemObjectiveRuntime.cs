using UnityEngine;

public class SubmitItemObjectiveRuntime : ObjectiveRuntime
{
    private ItemSO targetItem;
    private int requiredAmount;
    private int submittedAmount;

    public override int CurrentAmount => submittedAmount;

    public override int RequiredAmount => requiredAmount;

    public SubmitItemObjectiveRuntime(ItemSO targetItem, int requiredAmount, string description)
    {
        this.targetItem = targetItem;
        this.requiredAmount = requiredAmount;
        this.description = description;
    }

    public override void Activate()
    {
        State = ObjectiveState.Active;
    }

    public override void Deactivate()
    {
    }

    public bool TrySubmit()
    {
        if(State == ObjectiveState.Completed)
        return false;

        if(!InventoryManager.Instance.HasItem(targetItem, requiredAmount))
        {
            Debug.Log("Not enough items to submit.");
            return false;
        }

        bool removed = InventoryManager.Instance.RemoveItem(targetItem, requiredAmount);

        if(!removed) return false;

        Complete();
        return true;
    }

    private void Complete()
    {
        submittedAmount = requiredAmount;
        NotifyProgress();
        State = ObjectiveState.Completed;

        Debug.Log($"Submitted item : {targetItem.itemName} x{requiredAmount}");

        NotifyCompleted();
    }
}
