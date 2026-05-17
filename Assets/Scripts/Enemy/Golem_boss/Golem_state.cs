using System;
using UnityEngine;

public class Golem_state : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public float walkSpeed;
    public float attackCooldown;
    public float attackRange;
    public float knockbackForce;
    public float knockbackDuration;
    public float detectionRange;




    private GolemState currentState;
    public GolemState CurrentState => currentState;
    private Animator anim;


    private void Start()
    {
        currentHealth = maxHealth;
        currentState = GolemState.Idle;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    public void ChangeState(GolemState newState)
    {
        Debug.Log("ChangeState called with " + newState);
        if (currentState == newState)
            return;

        Debug.Log($"Golem state changed from {currentState} to {newState}");
        currentState = newState;

        if (currentState == GolemState.Attacking)
            anim.SetTrigger("Attack");
    }

    private void UpdateAnimator()
    {
        anim.SetBool("isWalking", currentState == GolemState.Walking);
    }

    public enum GolemState
    {
        Idle,
        Walking,
        Attacking,
        Hurt,
        Dead
    }
}