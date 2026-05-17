using System.Collections;
using UnityEngine;


[System.Serializable]
public class FaceTargetStep : SceneStep
{
    public string actorId;

    public string targetActorId;

    public override IEnumerator Execute(SceneDirector director)
    {
        SceneActor actor = SceneRegistry.Instance.Get<SceneActor>(actorId);
        SceneActor target = SceneRegistry.Instance.Get<SceneActor>(targetActorId);

        if(actor == null || target == null)
        {
            yield break;
        }

        FacingController facing = actor.GetComponent<FacingController>();

        if(facing == null)
        {
            Debug.LogWarning($"Actor {actorId} does not have a FacingController component.");
            yield break;
        }

        float direction = target.transform.position.x - actor.transform.position.x;

        facing.FaceDirection(direction);

        yield return null;
    }
}