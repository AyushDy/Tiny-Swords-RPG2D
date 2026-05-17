using System.Collections;
using UnityEngine;


[System.Serializable]
public class MoveActorStep : SceneStep
{
    public string actorId;

    public string destination;

    public float speed = 3f;

    public override IEnumerator Execute(SceneDirector director)
    {
        SceneActor actor = SceneRegistry.Instance.Get<SceneActor>(actorId);
        SceneMarker destinationMarker = SceneRegistry.Instance.Get<SceneMarker>(destination);


        if (actor == null || destination == null)
        {
            yield break;
        }

        PlayerSceneMover mover = actor.GetComponent<PlayerSceneMover>();

        if (mover == null)
        {
            yield break;
        }

        actor.SetSceneControl(true);

        mover.MoveTo(destinationMarker.transform.position);

        yield return new WaitUntil(() => mover.HasReachedDestination());

        mover.Stop();
        
        actor.SetSceneControl(false);
    }
}
