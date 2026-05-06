using System;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{

    public Image fillImage;
    public static StatsManager Instance;
    [Header("Combat Stats")]
    public int damage;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;


    [Header("Movement Stats")]
    public float speed;
    public float walkingSpeed;


    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    public static event Action<int, int> OnHealthChanged;




    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        RefreshHealthUI();
    }


    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        RefreshHealthUI();
    }


    public void SetHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        RefreshHealthUI();
    }

    private void RefreshHealthUI()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = (float)currentHealth / maxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }


    public void UpdateMaxHealth(int amount)
    {
        maxHealth = Mathf.Max(1, maxHealth + amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        RefreshHealthUI();
    }
}
