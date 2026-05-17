using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Golem_movement : MonoBehaviour
{
    public Transform detectionPoint;



    private NavMeshAgent agent;
    private CircleCollider2D detectionRange;
    private Animator anim;
    private Rigidbody2D rb;
    private int facingDirection;
    private Golem_state golemState;
    private Transform player;




    public float flipWaitDuration = 0.5f;


    private float attackTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detectionRange = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        golemState = GetComponent<Golem_state>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        facingDirection = transform.localScale.x > 0 ? 1 : -1;

        attackTimer = 0;
    }

    void Update()
    {
        CheckForPlayer();

        attackTimer -= Time.deltaTime;
    }

    public void StopMoving()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector2.zero;
    }


    private void ChasePlayer(Transform player)
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        golemState.ChangeState(Golem_state.GolemState.Walking);
        HandleFlip(player);
    }

    private void HandleFlip(Transform player)
    {
        if ((player.position.x < transform.position.x && facingDirection == 1) ||
            (player.position.x > transform.position.x && facingDirection == -1))
        {
            Flip();
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, golemState.detectionRange, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            player = hits[0].transform;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= golemState.attackRange && golemState.CurrentState != Golem_state.GolemState.Attacking && attackTimer <= 0)
            {
                golemState.ChangeState(Golem_state.GolemState.Attacking);
                anim.SetTrigger("Attack");
                attackTimer = golemState.attackCooldown;
                StopMoving();
            }
            else if (golemState.CurrentState != Golem_state.GolemState.Attacking)
            {
                ChasePlayer(player);
            }
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingDirection *= -1;
    }
}
