using UnityEngine;

public class EventBusTester : MonoBehaviour
{
    private void Start()
    {
        EventBus.Subscribe<EnemyKilledEvent>(
            "wolf",
            OnWolfKilled
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Wolf died");

            EventBus.Publish(
                "wolf",
                new EnemyKilledEvent("wolf")
            );
        }
    }

    private void OnWolfKilled(object data)
    {
        EnemyKilledEvent enemyKilledEvent = (EnemyKilledEvent)data;
        Debug.Log($"Received event for : {enemyKilledEvent.enemyId}");
    }


    private void OnDestroy()
    {
        EventBus.Unsubscribe<EnemyKilledEvent>(
            "wolf",
            OnWolfKilled
        );
    }


}
