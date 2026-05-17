using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class WaitStep : SceneStep
{
    public float duration;

    public override IEnumerator Execute(SceneDirector director)
    {
        yield return new WaitForSeconds(duration);
    }
}