using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCState {Default, Idle, Patrol, Talk, Wander}
    public NPCState currentState;
    private NPCState defaultState;


    public NPC_patrol patrolScript;
    public NPC_talk talkScript;
    public NPC_wander wanderScript;


    private void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        currentState = newState;

        patrolScript.enabled = currentState == NPCState.Patrol;
        talkScript.enabled = currentState == NPCState.Talk;
        wanderScript.enabled = currentState == NPCState.Wander;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SwitchState(NPCState.Talk);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SwitchState(defaultState);
        }
    }
}
