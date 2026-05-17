using System.Collections;
using UnityEngine;


[System.Serializable]
public class SceneObjectActiveStep : SceneStep
{
    public string objectId;

    public bool active;

    public override IEnumerator Execute(SceneDirector director)
    {
        SceneReference reference = SceneRegistry.Instance.Get<SceneReference>(objectId);

        if(reference == null)
        {
            yield break;
        }

        reference.gameObject.SetActive(active);

        yield return null;
    }
}

