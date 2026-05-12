using UnityEngine;

public class KillEnemyObjectiveRuntime : ObjectiveRuntime
{
    private string enemyId;
    private int requiredKills;
    private int currentKills;

    public override int CurrentAmount => currentKills;
    public override int RequiredAmount => requiredKills;


    public KillEnemyObjectiveRuntime(string enemyId, int requiredKills)
    {
        this.enemyId = enemyId;
        this.requiredKills = requiredKills;
    }

    public override void Activate()
    {
        State = ObjectiveState.Active;

        EventBus.Subscribe<EnemyKilledEvent>(enemyId, OnEnemyKilled);

        Debug.Log($"[Objective] listening for kills : {enemyId}");
    }

    public override void Deactivate()
    {
        EventBus.Unsubscribe<EnemyKilledEvent>(enemyId, OnEnemyKilled);

        Debug.Log($"[Objective] stopped listening for kills : {enemyId}");
    }

    private void OnEnemyKilled(object data)
    {
        if (State == ObjectiveState.Completed)
        {
            return;
        }

        currentKills++;

        Debug.Log(
            $"[Objective] {enemyId} killed. Current kills : {currentKills}/{requiredKills}"
        );

        if (currentKills >= requiredKills)
        {
            Complete();
        }
    }


    private void Complete()
    {
        State = ObjectiveState.Completed;

        Debug.Log($"[Objective] Kill {enemyId} objective completed!");

        NotifyCompleted();
        Deactivate();
    }
}
