using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene System/Scene Sequence")]
public class SceneSequenceSO : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    public List<SceneStep> steps = new();
}