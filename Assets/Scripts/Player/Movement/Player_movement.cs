using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player_movement : MonoBehaviour
{

    public int facingDirection = 1;
    public Rigidbody2D rb;
    public Animator anim;
    public Player_combat Player_combat;
    public Player_dash Player_dash;
    public Player_state Player_state;

    float axisBufferTime = 0.1f;
    float lastHorizontal;
    float lastVertical;
    float lastHorizontalTime;
    float lastVerticalTime;



    private bool isKnockedBack;


    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            lastHorizontal = horizontal;
            lastHorizontalTime = Time.time;
        }
        if (vertical != 0)
        {
            lastVertical = vertical;
            lastVerticalTime = Time.time;
        }

        float finalHorizontal = horizontal;
        float finalVertical = vertical;

        if (horizontal == 0 && Time.time - lastHorizontalTime <= axisBufferTime)
        {
            finalHorizontal = lastHorizontal;
        }

        if (vertical == 0 && Time.time - lastVerticalTime <= axisBufferTime)
        {
            finalVertical = lastVertical;
        }

        Vector2 moveDirection = new Vector2(finalHorizontal, finalVertical).normalized;

        if (moveDirection != Vector2.zero)
        {
            Player_dash.lastMoveDirection = moveDirection;
        }

        if (Player_dash.IsDashing || isKnockedBack)
        {
            return;
        }
        else if ((!Player_state.CanMove) || Player_state.Action == ActionState.Defending)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        else if (Player_state.Action == ActionState.Attacking)
        {
            return;
        }
        else if (!isKnockedBack && !Player_dash.IsDashing)
        {
            if (((horizontal > 0 && facingDirection < 0) || (horizontal < 0 && facingDirection > 0)) && Player_state.Locomotion != LocomotionState.Aiming)
            {
                Flip();
            }
            else if (Player_state.Locomotion == LocomotionState.Aiming)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((mousePosition.x > transform.position.x && facingDirection < 0) || (mousePosition.x < transform.position.x && facingDirection > 0))
                {
                    Flip();
                }
            }
            bool movingBackward = (finalHorizontal > 0 && facingDirection < 0) ||
                                 (finalHorizontal < 0 && facingDirection > 0);

            float currentSpeed = movingBackward ? StatsManager.Instance.walkingSpeed : StatsManager.Instance.speed;

            anim.SetFloat("horizontal", Mathf.Abs(finalHorizontal));
            anim.SetFloat("vertical", Mathf.Abs(finalVertical));

            rb.linearVelocity = moveDirection.normalized * currentSpeed;
        }
    }

    public void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.lossyScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


    public void Knockback(Transform enemy, float knockbackForce, float stunDuration)
    {
        Player_state.TrySetLocomotion(LocomotionState.Knockback);
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        if (Player_state.Locomotion != LocomotionState.Dead)
        {
            StartCoroutine(KnockbackCounter(stunDuration));
        }
    }

    IEnumerator KnockbackCounter(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
        Player_state.TrySetLocomotion(LocomotionState.Normal, true);
    }
}
