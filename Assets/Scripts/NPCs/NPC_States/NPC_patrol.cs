using UnityEngine;
using System.Collections;
using System.Linq;

public class NPC_patrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    public float speed = 2.5f;
    public float pauseDuration = 2f;


    private bool isMoving;
    private Vector2 target;
    private int currentPatrolIndex;
    private Rigidbody2D rb;
    private Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    void OnEnable()
    {
        if (patrolPoints.Length > 0)
            StartCoroutine(SetPatrolPoint());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
        isMoving = false;
        anim.SetBool("isMoving", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = target - (Vector2)transform.position;
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rb.linearVelocity = direction.normalized * speed;
        if (isMoving && Vector2.Distance(transform.position, target) < 0.1f)
            StartCoroutine(SetPatrolPoint());
    }

    IEnumerator SetPatrolPoint()
    {
        isMoving = false;
        anim.SetBool("isMoving", isMoving);
        yield return new WaitForSeconds(pauseDuration);
        Debug.Log("Setting new patrol point" + target);
        currentPatrolIndex++;
        target = patrolPoints[currentPatrolIndex % patrolPoints.Length];
        isMoving = true;
        anim.SetBool("isMoving", isMoving);
    }
}
