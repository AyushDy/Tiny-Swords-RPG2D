using UnityEngine;
using UnityEngine.Assertions.Must;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifeSpan = 3;
    public float speed = 10;
    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;

    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    
    public SpriteRenderer sr;
    public Sprite burriedSprite;


    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifeSpan);
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Enemy_health>().ChangeHealth(-damage);
            collision.gameObject.GetComponent<Enemy_knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
            AttachToTarget(collision.gameObject.transform);
    }

    private void AttachToTarget(Transform target)
    {
        sr.sprite = burriedSprite;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType =RigidbodyType2D.Kinematic;

        transform.SetParent(target);
    }
}