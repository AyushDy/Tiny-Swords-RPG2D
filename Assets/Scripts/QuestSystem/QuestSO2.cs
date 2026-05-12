using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO2", menuName = "Quest/Quest")]
public class QuestSO2 : ScriptableObject
{
    [Header("Identity")]
    public string questId;

    [Header("Display")]
    public string questName;
    [TextArea(3, 10)] public string questDescription;

    [Header("Objectives")]
    public List<QuestObjectiveDefinition> objectives = new List<QuestObjectiveDefinition>();

    [Header("Rewards")]
    public List<QuestRewardDefinition> rewards = new List<QuestRewardDefinition>();
}

public enum QuestObjectiveType
{
    KillEnemy,
    CollectItem,
    SubmitItem,
    Dialogue
}

[Serializable]
public class QuestObjectiveDefinition
{
    public QuestObjectiveType objectiveType;

    [TextArea]
    public string description;

    [Tooltip("Used for KillEnemy and Dialogue objectives.")]
    public string targetId;

    [Tooltip("Used for CollectItem and SubmitItem objectives.")]
    public ItemSO targetItem;

    [Min(1)]
    public int requiredAmount = 1;
}

[Serializable]
public class QuestRewardDefinition
{
    public ItemSO itemSO;
    public int quantity = 1;
}