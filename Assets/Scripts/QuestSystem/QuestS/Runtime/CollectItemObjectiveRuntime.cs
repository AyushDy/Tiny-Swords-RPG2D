using UnityEngine;

public class CollectItemObjectiveRuntime : ObjectiveRuntime
{
    private ItemSO targetItem;

    private int requiredAmount;
    private int currentAmount;

    public override int CurrentAmount => currentAmount;
    public override int RequiredAmount => requiredAmount;

    public CollectItemObjectiveRuntime(ItemSO item, int amount)
    {
        targetItem = item;
        requiredAmount = amount;
    }

    public override void Activate()
    {
        State = ObjectiveState.Active;

        EventBus.Subscribe<ItemCollectedEvent>(
            targetItem.itemId,
            OnItemCollected
        );

        Debug.Log($"Listening for item collection: {targetItem.itemName}");
    }

    public override void Deactivate()
    {
        EventBus.Unsubscribe<ItemCollectedEvent>(
            targetItem.itemId,
            OnItemCollected
        );
        Debug.Log($"Stopped listening for item collection: {targetItem.itemName}");
    }

    private void OnItemCollected(object data)
    {
        if(State == ObjectiveState.Completed)
            return;
        
        ItemCollectedEvent itemEvent =  (ItemCollectedEvent)data;

        currentAmount += itemEvent.amount;

        if(currentAmount >= requiredAmount)
        {
            currentAmount = requiredAmount;
        }

        Debug.Log($"Collected {targetItem.itemId} : {currentAmount}/{requiredAmount}");

        if(currentAmount >= requiredAmount)
        {
            Complete();
        }
    }


    private void Complete()
    {
        State = ObjectiveState.Completed;
        Debug.Log($"[Objective] Collection objective completed : {targetItem.itemId}");

        NotifyCompleted();

        Deactivate();
    }
}
