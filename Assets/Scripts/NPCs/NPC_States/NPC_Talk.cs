using System.Collections.Generic;
using UnityEngine;

public class NPC_talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactionAnim;
    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueOptionSelected += HandleDialogueOptionSelected;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetBool("isMoving", false);
        interactionAnim.Play("Open");
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueOptionSelected -= HandleDialogueOptionSelected;
        interactionAnim.Play("Close");
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else
            {
                if (DialogueManager.Instance.canStartDialogue())
                {
                    CheckForNewConversation();
                    DialogueManager.Instance.StartDialogue(currentConversation);
                }
            }
        }
    }

    private void CheckForNewConversation()
    {
        for (int i = 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            if (convo != null && convo.isConditionMet())
            {
                currentConversation = convo;
                if (convo.RemoveAfterPlay)
                    conversations.RemoveAt(i);
                if (convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0)
                {
                    foreach (var convoToRemove in convo.removeTheseOnPlay)
                    {
                        conversations.Remove(convoToRemove);
                    }
                }
                return;
            }
        }
    }

    private void HandleDialogueOptionSelected(DialogueSO dialogueSO)
    {
        var convo = dialogueSO;
        if (convo.isConditionMet())
        {
            currentConversation = convo;

            if (convo.RemoveAfterPlay)
                conversations.Remove(convo);

            if (convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0)
            {
                foreach (var convoToRemove in convo.removeTheseOnPlay)
                {
                    Debug.Log("Removing conversation: " + convoToRemove.name);
                    conversations.Remove(convoToRemove);
                }
            }
            return;
        }
    }
}
