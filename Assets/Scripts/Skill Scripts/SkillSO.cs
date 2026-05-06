using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "SkillTree/Skill")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public int maxLevel;
    public Sprite skillIcon;
    public SkillType skillType;


    public enum SkillType
    {
        MaxHealth,
        Shockwave,
        DamageBoost
    }
}
