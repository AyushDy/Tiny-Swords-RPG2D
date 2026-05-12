using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private QuestSO questToOffer;
    [SerializeField] private QuestSO questToTurnIn;

    private bool playerInRange;

    void Update()
    {
        // if (playerInRange && Input.GetButtonDown("Interact"))
        // {
        //     bool canTurnInQuest = questToTurnIn != null && QuestEvents.IsQuestComplete?.Invoke(questToTurnIn) == true;

        //     if (canTurnInQuest)
        //     {
        //         QuestEvents.onQuestTurnInRequested?.Invoke(questToTurnIn);
        //     }

        //     else
        //     {
        //         QuestEvents.onQuestOfferRequested?.Invoke(questToOffer);
        //     }
        // }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
