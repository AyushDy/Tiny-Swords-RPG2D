using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExpManager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToLevel = 10;
    public float expMultiplier = 1.5f;
    public Slider expSlider;


    public static event Action<int> OnLevelUp;

    public void Start()
    {
        UpdateUI();
    }

    public void OnEnable()
    {
        Enemy_health.OnMonsterDefeated += GainExperience;
        InventoryManager.OnExperienceGained += GainExperience;
    }

    public void OnDisable()
    {
        Enemy_health.OnMonsterDefeated -= GainExperience;
        InventoryManager.OnExperienceGained -= GainExperience;
    }
    
    public void GainExperience(int amount)
    {
        currentExp += amount;
        while (currentExp >= expToLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * expMultiplier);
        OnLevelUp?.Invoke(1);
    }


    public void UpdateUI()
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
    }
}

