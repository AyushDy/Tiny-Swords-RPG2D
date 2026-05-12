using UnityEngine;

public class QuestTester : MonoBehaviour
{
    public QuestSO2 testQuest;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            EventBus.Publish(
                "wolf",
                new EnemyKilledEvent("wolf")
            );
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            QuestManager2.Instance.StartQuest(testQuest);
        }
    }
}