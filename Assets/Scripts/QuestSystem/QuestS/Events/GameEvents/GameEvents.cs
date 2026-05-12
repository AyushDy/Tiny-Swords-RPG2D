public struct EnemyKilledEvent
{
    public readonly string enemyId;

    public EnemyKilledEvent(string enemyId)
    {
        this.enemyId = enemyId;
    }
}


public struct ItemCollectedEvent
{
    public readonly string itemId;
    public readonly int amount;

    public ItemCollectedEvent(string itemId, int itemAmount)
    {
        this.itemId = itemId;
        this.amount = itemAmount;
    }
}

public struct DialogueFinishedEvent
{
    public readonly string npcId;
    public readonly string dialogueId;

    public DialogueFinishedEvent(string npcId, string dialogueId)
    {
        this.npcId = npcId;
        this.dialogueId = dialogueId;
    }
}