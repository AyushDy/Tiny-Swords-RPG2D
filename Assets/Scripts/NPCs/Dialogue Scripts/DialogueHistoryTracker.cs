using System.Collections.Generic;
using UnityEngine;

public class DialogueHistoryTracker : MonoBehaviour
{
    public static DialogueHistoryTracker Instance;
    private readonly HashSet<ActorSO> spokenNPCs = new HashSet<ActorSO>();

    private void Awake()
    {
        if (Instance != null)
        {
           Destroy(gameObject);
           return;
        }
        else
           Instance = this;
    }

    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);
    }

    public bool HasSpokenWithNPC(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }
}
