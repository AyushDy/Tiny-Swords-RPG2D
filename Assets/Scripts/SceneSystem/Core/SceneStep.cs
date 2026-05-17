using System;
using System.Collections;


[Serializable]
public abstract class SceneStep
{
    public abstract IEnumerator Execute(SceneDirector director);
}
