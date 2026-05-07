using System.Collections;
using UnityEngine;

public class NPC_wander : MonoBehaviour
{
    [Header("Wander area")]
    public float wanderWidth = 5f;
    public float wanderHeight = 5f;
    public Vector2 startingPosition;

    public float speed = 2f;
    public float pauseDuration = 2f;
    public Vector2 target;


    private NPC npcScript;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isMoving;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        npcScript = GetComponent<NPC>();
    }

    void OnEnable()
    {
        StartCoroutine(PauseAndPickNewDestination());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
        isMoving = false;
        anim.SetBool("isMoving", false);
    }

    private void Update()
    {
        if (!isMoving)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(PauseAndPickNewDestination());
        }
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(npcScript.currentState == NPC.NPCState.Wander)
        StartCoroutine(PauseAndPickNewDestination());
    }


    IEnumerator PauseAndPickNewDestination()
    {
        isMoving = false;
        anim.SetBool("isMoving", isMoving);
        yield return new WaitForSeconds(pauseDuration);
        target = GetRandomTarget();
        isMoving = true;
        anim.SetBool("isMoving", isMoving);
    }

    private Vector2 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2f;
        float halfHeight = wanderHeight / 2f;
        int edge = Random.Range(0, 4);

        return edge switch
        {
            0 => new Vector2(startingPosition.x - halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)),
            1 => new Vector2(startingPosition.x + halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)),
            2 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y - halfHeight),
            _ => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y + halfHeight),
        };
    }


    private void Move()
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        if (dir.x < 0 && transform.localScale.x > 0 || dir.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rb.linearVelocity = dir * speed;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, wanderHeight, 0));
    }
}
