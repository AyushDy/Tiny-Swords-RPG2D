using UnityEngine;

public class SceneAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void PlayTrigger(string triggerName)
    {
        Debug.Log($"Triggering: {triggerName}");
        anim.SetTrigger(triggerName);
    }
}
