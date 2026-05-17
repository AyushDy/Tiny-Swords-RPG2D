
using UnityEngine;

public class SceneReference : MonoBehaviour
{
    public string id;

    private void OnEnable()
    {
        SceneRegistry.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (SceneRegistry.Instance != null)
            SceneRegistry.Instance.Unregister(this);
    }
}
