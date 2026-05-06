using UnityEngine;

public class NPC_talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactionAnim;
    public DialogueSO dialogueSO;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetBool("isMoving", false);
        interactionAnim.Play("Open");
    }

    private void OnDisable()
    {
        interactionAnim.Play("Close");
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if(DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueSO);
            }
        }
    }
}
