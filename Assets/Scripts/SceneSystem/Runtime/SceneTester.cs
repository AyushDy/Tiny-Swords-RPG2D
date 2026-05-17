using UnityEngine;

public class SceneTester : MonoBehaviour
{
    public SceneSequenceSO testSequence;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneDirector.Instance.Play(testSequence);
        }
    }
}