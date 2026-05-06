using UnityEngine;

public class Player_AnimationHandler : MonoBehaviour
{
    public Animator anim;
    public Player_state state;


    void OnEnable()
    {
        state.OnStateChanged += UpdateStateAnimations;
    }

    void Update()
    {
        UpdateStateAnimations();
    }

    void OnDisable()
    {
        state.OnStateChanged -= UpdateStateAnimations;
    }

    public void UpdateStateAnimations()
    {
        bool isShooting = state.Action == ActionState.Shooting;
        bool isDefending = state.Action == ActionState.Defending;

        anim.SetBool("isShooting", isShooting);
        anim.SetBool("isDefending", isDefending);
    }

    public void TriggerParry()
    {
        anim.SetTrigger("Parry");
    }
}
