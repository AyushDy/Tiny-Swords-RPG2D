using System.Collections;
using UnityEngine;


[System.Serializable]
public class PlayObjectAnimationtep : SceneStep
{
    public string objectId;
    public string triggerName;
    public float waitDuration;

    public override IEnumerator Execute(SceneDirector director)
    {
        SceneAnimationPlayer animationPlayer = SceneRegistry.Instance.Get<SceneAnimationPlayer>(objectId);

        if(animationPlayer == null)
        {
            yield break;
        }

        animationPlayer.PlayTrigger(triggerName);

        if(waitDuration > 0f)
        {
            yield return new WaitForSeconds(waitDuration);
        }
    }
}
