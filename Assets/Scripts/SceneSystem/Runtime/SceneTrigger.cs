using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public SceneSequenceSO sequence;

    private bool hasPlayed;

    private void OnTriggerEnter(Collider collision)
    {
        if (hasPlayed) return;

        if (collision.CompareTag("Player"))
        {
            SceneDirector.Instance.Play(sequence);
            hasPlayed = true;
        }
    }
}
