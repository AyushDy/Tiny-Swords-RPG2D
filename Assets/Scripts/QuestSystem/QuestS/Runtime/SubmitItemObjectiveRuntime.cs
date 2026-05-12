using UnityEngine;

public class SubmitItemObjectiveRuntime : ObjectiveRuntime
{
    private ItemSO targetItem;
    private int requiredAmount;

    public override int CurrentAmount
    {
        get
        {
            return InventoryManager.Instance.GetItemQuantity(targetItem);
        }
    }

    public override int RequiredAmount => requiredAmount;

    public SubmitItemObjectiveRuntime(ItemSO targetItem, int requiredAmount)
    {
        this.targetItem = targetItem;
        this.requiredAmount = requiredAmount;
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
        State = ObjectiveState.Completed;

        Debug.Log($"Submitted item : {targetItem.itemName} x{requiredAmount}");

        NotifyCompleted();
    }
}
