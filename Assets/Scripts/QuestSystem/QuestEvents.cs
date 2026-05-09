using System;
using UnityEngine;

public static class QuestEvents
{
    public static Action<QuestSO> onQuestOfferRequested;
    public static Action<QuestSO> onQuestTurnInRequested;
    public static Func<QuestSO, bool> IsQuestComplete;
}