using UnityEngine;
using UnityEngine.AI;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 1;
    public float playerDetectionRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    public AudioSource audioSource;
    public AudioClip chaseSound;
    public float chaseSoundCooldown = 12f;




    private float attackCooldownTimer;
    private int facingDirection = -1;
    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private NavMeshAgent agent;
    private float chaseSoundTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
            agent.velocity = agent.desiredVelocity;
            CheckForPlayer();
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                agent.isStopped = true;
            }
        }

        if (agent.velocity.magnitude > 0.1f)
        {
            anim.SetBool("isChasing", true);
        }
        else
        {
            anim.SetBool("isChasing", false);
        }
        if (chaseSoundTimer > 0)
            chaseSoundTimer -= Time.deltaTime;
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectionRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            // if the player is in attack range AND attack cooldown is 0
            if (Vector2.Distance(transform.position, player.position) < attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    void Chase()
    {
        if (player.position.x > transform.position.x && facingDirection == -1 ||
               player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void ChangeState(EnemyState newState)
    {
        //Exit the current animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", false);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", false);
        }

        //Update our current state
        enemyState = newState;


        //update the new animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", true);

            if (audioSource != null && chaseSound != null && !audioSource.isPlaying && chaseSoundTimer <= 0)
            {
                audioSource.PlayOneShot(chaseSound);
                chaseSoundTimer = chaseSoundCooldown;
            }
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", true);
        }
    }

}


public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
}