using System.Collections;
using UnityEngine;



[System.Serializable]
public class FaceDirectionStep : SceneStep
{
    public string actorId;

    [Tooltip("-1 for left, 1 for right")]
    public int direction;

    public override IEnumerator Execute(SceneDirector director)
    {
        SceneActor actor = SceneRegistry.Instance.Get<SceneActor>(actorId);
        if(actor == null)
        {
            yield break;
        }

        FacingController facing = actor.GetComponent<FacingController>();

        if(facing == null)
        {
            Debug.LogWarning($"Actor {actorId} does not have a FacingController component.");
            yield break;
        }

        facing.SetFacing(direction);
        yield break;

    }
}
