using UnityEngine;
using UnityEngine.AI;

public class PlayerSceneMover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody2D rb;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.enabled = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!agent.enabled)
        {
            return;
        }
        Vector2 veclocity = agent.velocity;
        rb.linearVelocity = agent.desiredVelocity;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }

    public bool HasReachedDestination()
    {
        if (!agent.enabled)
            return false;

        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public void Stop()
    {
        if (!agent.enabled)
        {
            return;
        }
        agent.isStopped = true;

        rb.linearVelocity = Vector2.zero;
        
        agent.enabled = false;
    }
}
