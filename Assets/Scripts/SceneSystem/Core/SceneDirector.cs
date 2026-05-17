using System.Collections;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public static SceneDirector Instance;
    public DialogueManager dialogueManager;

    private bool isPlaying;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(SceneSequenceSO sequence)
    {
        if (isPlaying)
        {
            Debug.LogWarning("SceneDirector is already playing a sequence.");
            return;
        }
        StartCoroutine(PlayRoutine(sequence));
    }

    private IEnumerator PlayRoutine(SceneSequenceSO sequence)
    {
        isPlaying = true;

        for (int i = 0; i < sequence.steps.Count; i++)
        {
            yield return sequence.steps[i].Execute(this);
        }

        Debug.Log("Scene finished.");

        isPlaying = false;
    }
}
