using System;
using TMPro;
using UnityEngine;

public class QuestObjectiveSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text progressText;



    public void RefreshObjectives(string description, string progress, bool isComplete)
    {
        objectiveText.text = description;
        progressText.text = progress;

        Color color = isComplete ? Color.green : Color.white;
        objectiveText.color = color;
        progressText.color = color;
    }
}
