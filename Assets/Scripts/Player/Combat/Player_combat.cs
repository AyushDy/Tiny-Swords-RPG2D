using System.Collections;
using UnityEngine;

public class Player_combat : MonoBehaviour
{

    public static Player_combat instance;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public GameObject shockwavePrefab;
    public Player_dash playerDash;
    public SkillManager skillManager;
    public Player_state playerState;
    public Player_AnimationHandler animationHandler;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator anim;

    public AudioSource audioSource;
    public AudioClip slashSound;

    public int comboStep = 0;
    public bool canQueueNext = false;
    public bool queuedNext = false;

    public bool canReceiveInput;
    public bool inputReceived;


    private void Start()
    {
        instance = this;
    }




    // public void Attack()
    // {
    //     if (playerState.Equipment != EquipmentState.Sword) return;

    //     if (playerState.Action == ActionState.Attacking)
    //     { 
    //         if( comboStep == 0)
    //         {
    //             comboStep = 1;
    //             canQueueNext = false;
    //             queuedNext = false;
    //             anim.SetBool("isAttacking", true);
    //             anim.SetInteger("ComboStep", comboStep);
    //         }
    //         if (canQueueNext && comboStep == 1)
    //             queuedNext = true;
    //         return;
    //     }

    //     comboStep = 1;
    //     canQueueNext = false;
    //     queuedNext = false;

    //     anim.SetBool("isAttacking", true);
    //     anim.SetInteger("ComboStep", comboStep);

    //     if (skillManager.isShockwaveActive)
    //     {
    //         StartCoroutine(SpawnShockwave());
    //     }
    // }


    public void CheckAttackEffects()
    {
        if (playerState.Equipment != EquipmentState.Sword) return;

        if(skillManager.isShockwaveActive)
        {
            StartCoroutine(ShockwaveRoutine());
        }
    }


    public void Attack()
    {
        if (canReceiveInput)
        {
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }
    }

    public void InputManager2()
    {
        if (canReceiveInput)
        {
            canReceiveInput = false;
        }
        else
        {
            canReceiveInput = true;
        }
    }



    public void PlayAttackSound()
    {
        if (slashSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(slashSound);
        }
    }


    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<Enemy_health>().ChangeHealth(-StatsManager.Instance.damage);
            enemies[0].GetComponent<Enemy_knockback>().Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
        }
    }

    public void EnableComboWindow()
    {
        canQueueNext = true;
    }

    public void CheckCombo()
    {
        canQueueNext = false;

        if (queuedNext)
        {
            comboStep = 2;
            anim.SetInteger("ComboStep", comboStep);
            queuedNext = false;
        }
        else
        {
            EndAttack();
        }
    }


    public void EndAttack()
    {
        anim.SetBool("isAttacking", false);
        anim.SetInteger("ComboStep", 0);

        comboStep = 0;
        canQueueNext = false;
        queuedNext = false;

        if (playerState.Action == ActionState.Attacking)
        {
            playerState.EndAction();
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        float range = StatsManager.Instance == null ? 1.0f : StatsManager.Instance.weaponRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, range);
    }

    public void SpawnShockwave()
    {
        StartCoroutine(ShockwaveRoutine());
    }


    IEnumerator ShockwaveRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject shockwave = Instantiate(shockwavePrefab, attackPoint.position, Quaternion.identity);
        Vector2 direction = playerDash.lastMoveDirection;
        if (direction == Vector2.zero)
            direction = Vector2.right;
        shockwave.GetComponent<Schockwave_Script>().Initialise(direction);
    }
}
