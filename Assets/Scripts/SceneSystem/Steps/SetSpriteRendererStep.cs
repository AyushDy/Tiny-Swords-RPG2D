using System.Collections;
using UnityEngine;


[System.Serializable]
public class SetSpriteRendererStep : SceneStep
{
    public string objectId;
    public bool enabledState;

    public override IEnumerator Execute(SceneDirector director)
    {
        SpriteRenderer renderer = SceneRegistry.Instance.Get<SpriteRenderer>(objectId);

        if(renderer == null)
        {
            yield break;
        }
        renderer.enabled = enabledState;
        yield return null;
    }
}
