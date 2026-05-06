using UnityEngine;

public class Schockwave_Script : MonoBehaviour
{
    public float lifeSpan = 1;
    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public float speed;
    public LayerMask enemyLayer;
    public Rigidbody2D rb;


    public void Initialise(Vector2 direction)
    {
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.linearVelocity = direction * speed;

        Destroy(gameObject, lifeSpan);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Enemy_health>().ChangeHealth(-damage);
            collision.gameObject.GetComponent<Enemy_knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
    }


}
