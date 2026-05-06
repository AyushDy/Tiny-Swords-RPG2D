using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{

    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunDuration;
    public LayerMask playerLayer;


    public AudioSource audioSource;
    public AudioClip slashSound;


    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if(hits.Length> 0)
        {
            hits[0].GetComponent<Player_health>().changeHealth(-damage);
            hits[0].GetComponent<Player_movement>().Knockback(transform, knockbackForce, stunDuration);
        }
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(slashSound);
    }
}
