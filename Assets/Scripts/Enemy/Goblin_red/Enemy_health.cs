using Unity.Cinemachine;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    public string enemyId = EnemyIds.GoblinRed;

    public int expReward = 5;
    public delegate void MonsterDefeated(int expReward);
    public static event MonsterDefeated OnMonsterDefeated;

    public int currentHealth;
    public int maxHealth;
    public CinemachineImpulseSource impulseSource;
    public EnemyHitFlash hitFlash;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void ChangeHealth(int amount)
    {
        impulseSource.GenerateImpulse();
        hitFlash.PlayHitEffect();
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            OnMonsterDefeated(expReward);

            EventBus.Publish(enemyId,new EnemyKilledEvent(enemyId));

            Destroy(gameObject);
        }
    }
}
