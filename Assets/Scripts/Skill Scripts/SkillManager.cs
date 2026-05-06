using System.Collections;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public bool isShockwaveActive;
    float shockWaveDuration = 5f;
    float shockWaveCooldown = 10f;
    float shockWaveCooldownTimer;
    private bool isShockwaveUnlocked = false;

    void Update()
    {
        if(Input.GetButtonDown("ActivateShockwave"))
        {
            ActivateShockwave();
        }
        if(shockWaveCooldownTimer > 0)
        {
            shockWaveCooldownTimer -= Time.deltaTime;
        }
    }


    void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointsSpent;
    }

    void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointsSpent;
    }

    private void HandleAbilityPointsSpent(SkillSlot slot)
    {
        switch (slot.skillSO.skillType)
        {
            case SkillSO.SkillType.MaxHealth:
                StatsManager.Instance.UpdateMaxHealth(2);
                break;
            case SkillSO.SkillType.Shockwave:
                isShockwaveUnlocked = true;
                break;
            default:
                Debug.LogWarning("Unknown skill: " + slot.skillSO.skillName);
                break;
        }
    }


    void ActivateShockwave()
    {
        if(!isShockwaveUnlocked || shockWaveCooldownTimer > 0)
            return;
        Debug.Log("Shockwave Activated!");
        StartCoroutine(CooldownShockwave());
    }


    IEnumerator CooldownShockwave()
    {
        isShockwaveActive = true;
        Debug.Log("Shockwave is active for " + shockWaveDuration + " seconds.");
        Debug.Log(isShockwaveActive);
        yield return new WaitForSeconds(shockWaveDuration);
        isShockwaveActive = false;
        shockWaveCooldownTimer = shockWaveCooldown;
    } 
}
