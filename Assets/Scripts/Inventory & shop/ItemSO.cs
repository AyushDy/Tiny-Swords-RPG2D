using UnityEngine;


[CreateAssetMenu(fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    public bool isGold;
    public bool isExp;
    public int stackSize = 3;

    [Header("Runtime")]
    public string itemId;

    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public int speed;
    public int damage;


    [Header("For temporary items")]
    public float duration;
}
