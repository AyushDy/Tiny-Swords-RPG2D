using UnityEngine;

public class QuestTester : MonoBehaviour
{
    public QuestSO2 testQuest;

    private void OnEnable()
    {
        QuestManager2.Instance.OnQuestOfferReceived += OnQuestOfferReceived;
    }

    private void OnDisable()
    {
        QuestManager2.Instance.OnQuestOfferReceived -= OnQuestOfferReceived;
    }

    private void OnQuestOfferReceived(QuestSO2 quest)
    {
        Debug.Log($"[Tester] Offer event received for: {quest.questId}");
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     EventBus.Publish(
        //         "wolf",
        //         new EnemyKilledEvent("wolf")
        //     );
        // }

        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     QuestManager2.Instance.StartQuest(testQuest);
        // }

        if (Input.GetKeyDown(KeyCode.N))
        {
            QuestManager2.Instance.StartQuest(testQuest);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            QuestManager2.Instance.TryTurnInQuest(testQuest.questId);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            QuestRuntime quest = QuestManager2.Instance.GetQuest(testQuest.questId);
            if (quest != null)
            {
                quest.Fail();
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            QuestManager2.Instance.AcceptPendingQuestOffer();
        }
    }
}