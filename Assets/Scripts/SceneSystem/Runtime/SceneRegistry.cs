using System.Collections.Generic;
using UnityEngine;

public class SceneRegistry : MonoBehaviour
{
    private static SceneRegistry instance;

    public static SceneRegistry Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<SceneRegistry>();
            }
            return instance;
        }
    }


    private Dictionary<string, SceneReference> references = new ();

    public void Register(SceneReference reference)
    {
        if(references.ContainsKey(reference.id))
        {
            Debug.LogWarning($"SceneRegistry: Duplicate id '{reference.id}' found. Overwriting existing reference.");
        }
        references[reference.id] = reference;
    }

    public void Unregister(SceneReference reference)
    {
        if(references.ContainsKey(reference.id))
        {
            references.Remove(reference.id);
        }
    }

    public T Get<T>(string id) where T : Component
    {
        if(references.TryGetValue(id, out SceneReference reference))
        {
            return reference.GetComponent<T>();
        }
        return null;
    }
}
