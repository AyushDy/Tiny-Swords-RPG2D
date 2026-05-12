using System;
using UnityEngine;

public static class QuestEvents
{
    public static Action<QuestSO2> onQuestOfferRequested;
    public static Action<QuestSO2> onQuestTurnInRequested;
    public static Action<QuestSO2> onQuestAccepted;
    public static Func<QuestSO2, bool> IsQuestComplete;
}