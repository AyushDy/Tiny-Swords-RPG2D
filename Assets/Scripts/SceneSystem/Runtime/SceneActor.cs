using UnityEngine;

public class SceneActor : MonoBehaviour
{

    private bool sceneControlled;

    public bool IsSceneControlled => sceneControlled;

    public void SetSceneControl(bool value)
    {
        sceneControlled = value;
    }
}
