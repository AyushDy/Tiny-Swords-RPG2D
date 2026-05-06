using UnityEngine;
using System.Collections;

public class Player_health : MonoBehaviour
{

    private float invincibleUntilTime;
    private float invincibleDurationAfterHit = 1f;
    private Coroutine flashRoutine;
    public Player_state playerState;


    private SpriteRenderer sr;

    void OnDisable()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }

        if (sr != null)
            sr.enabled = true;
    }

    public void changeHealth(int amount)
    {
        if (amount < 0 && IsInvincible())
            return;

        if (amount < 0)
        {
            if (playerState.Action == ActionState.Defending)
            {
                if (playerState.isParryWindowActive())
                {
                    return;
                }
                amount = Mathf.RoundToInt(amount * 0.5f);
            }
        }

        StatsManager.Instance.ChangeHealth(amount);

        if (amount < 0)
        {
            StartInvincibility(invincibleDurationAfterHit);
        }

        if (StatsManager.Instance.currentHealth <= 0)
        {
            if (playerState != null)
                playerState.SetDead();
            gameObject.SetActive(false);
        }
    }


    public void StartInvincibility(float duration)
    {
        invincibleUntilTime = Mathf.Max(invincibleUntilTime, Time.time + duration);

        if (sr != null && flashRoutine == null)
        {
            flashRoutine = StartCoroutine(Flash());
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private bool IsInvincible()
    {
        return Time.time < invincibleUntilTime;
    }

    IEnumerator Flash()
    {
        while (IsInvincible())
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        sr.enabled = true;
        flashRoutine = null;
    }
}
