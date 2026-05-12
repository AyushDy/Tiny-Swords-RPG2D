using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "QuestSO", menuName = "QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questName;
    [TextArea]public string questDescription;

    public List<QuestObjective> objectives;
    public List<QuestReward> rewards;
}


[Serializable]
public class QuestObjective
{
    public string description;

    [SerializeField] private UnityEngine.Object target;
    public ItemSO targetItem => target as ItemSO;
    public ActorSO targetNPC => target as ActorSO;
    public LocationSO targetLocation => target as LocationSO;

    public int requiredAmount;
    public int currentAmount;
}

[Serializable]
public class QuestReward
{
    public ItemSO itemSO;
    public int quantity;
}