using UnityEngine;

public class Golem_attack : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask playerLayer;
    public Golem_state golemState;





    private void Awake()
    {
        if (golemState == null)
            golemState = GetComponent<Golem_state>();

        if (playerLayer.value == 0)
            playerLayer = LayerMask.GetMask("Player");
    }


    public void Attack()
    {
        if (attackPoint == null || golemState == null)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, golemState.attackRange, playerLayer);

        if (hits.Length > 0)
        {
            if (hits[0].TryGetComponent(out Player_health playerHealth))
                playerHealth.changeHealth(-golemState.damage);

            if (hits[0].TryGetComponent(out Player_movement playerMovement))
                playerMovement.Knockback(transform, golemState.knockbackForce, golemState.knockbackDuration);
        }
    }

    public void EndAttack()
    {
        Debug.Log("EndAttack called, current state: " + golemState.CurrentState);
        if (golemState.CurrentState == Golem_state.GolemState.Attacking)
            golemState.ChangeState(Golem_state.GolemState.Idle);
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, golemState.attackRange);
    }
}
