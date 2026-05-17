using System;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentState { Sword, Bow };
public enum LocomotionState { Normal, Aiming, Dashing, Knockback, Dead };
public enum ActionState { None, Attacking, Shooting, Defending };

public class Player_state : MonoBehaviour
{
    private float ActionTimer;

    public EquipmentState Equipment { get; private set; } = EquipmentState.Sword;
    public LocomotionState Locomotion { get; private set; } = LocomotionState.Normal;
    public ActionState Action { get; private set; } = ActionState.None;
    public bool InputLocked { get; private set; }

    public event Action OnStateChanged;


    private float defendStartTime;
    public float parryWindow = 0.2f;


    int GetLocomotionPriority(LocomotionState state)
    {
        switch (state)
        {
            case LocomotionState.Dead: return 100;
            case LocomotionState.Knockback: return 80;
            case LocomotionState.Dashing: return 60;
            case LocomotionState.Aiming: return 40;
            case LocomotionState.Normal: return 0;
            default: return 0;
        }
    }

    int GetActionPriority(ActionState state)
    {
        switch (state)
        {
            case ActionState.None: return 0;
            case ActionState.Attacking: return 40;
            case ActionState.Shooting: return 40;
            default: return 0;
        }
    }


    public bool CanMove => Action != ActionState.Shooting &&
                             Locomotion != LocomotionState.Dashing &&
                            Locomotion != LocomotionState.Knockback &&
                            Locomotion != LocomotionState.Dead;
    public bool CanAttack => Equipment == EquipmentState.Sword &&
                              Action == ActionState.None &&
                              Locomotion != LocomotionState.Knockback &&
                              Locomotion != LocomotionState.Dashing &&
                              Locomotion != LocomotionState.Dead;
    public bool IsBusy => Action != ActionState.None ||
                          Locomotion == LocomotionState.Knockback ||
                          Locomotion == LocomotionState.Dashing;

    void Update()
    {
        if (Action != ActionState.None)
        {
            ActionTimer -= Time.deltaTime;
            if (ActionTimer <= 0)
                EndAction();
        }
    }

    private void Awake()
    {
        Equipment = EquipmentState.Sword;
        Locomotion = LocomotionState.Normal;
        Action = ActionState.None;
        OnStateChanged?.Invoke();
    }

    public bool TryStartAction(ActionState next, float duration = 0.5f)
    {
        //if dead, can't do anything
        if (Locomotion == LocomotionState.Dead) return false;

        if (Action == next && next == ActionState.Attacking)
        {
            ActionTimer = duration;
            OnStateChanged?.Invoke();
            return true;
        }

        //Can't defend with bow
        if (next == ActionState.Defending && Equipment == EquipmentState.Bow)
        {
            return false;
        }

        int currentPriority = GetActionPriority(Action);
        int nextPriority = GetActionPriority(next);

        //next action lower priority
        if (nextPriority < currentPriority)
            return false;

        //if trying to defend but currently attacking or shooting, fail
        if (Locomotion == LocomotionState.Dashing ||
            Locomotion == LocomotionState.Knockback)
            return false;

        //if trying to attack or shoot but currently defending, fail
        if (next == ActionState.Defending)
        {
            defendStartTime = Time.time;
        }

        Action = next;
        ActionTimer = duration;
        OnStateChanged?.Invoke();
        return true;
    }


    public void EndAction()
    {
        Action = ActionState.None;
        OnStateChanged?.Invoke();
    }


    public bool TrySetLocomotion(LocomotionState next, bool force = false)
    {
        if (Locomotion == LocomotionState.Dead)
            return false;

        if (!force)
        {
            int currentPriority = GetLocomotionPriority(Locomotion);
            int nextPriority = GetLocomotionPriority(next);

            //next state has lower priority, fail
            if (nextPriority < currentPriority)
                return false;
        }

        if (next == LocomotionState.Dashing && Action != ActionState.None)
        {
            EndAction();
        }

        {
            if (Action == ActionState.Attacking || Action == ActionState.Shooting)
                return false;
        }

        Locomotion = next;
        OnStateChanged?.Invoke();
        return true;
    }

    public void SetEquipment(EquipmentState next)
    {
        Equipment = next;

        if (Equipment == EquipmentState.Bow && Action == ActionState.Defending)
        {
            EndAction();
        }

        OnStateChanged?.Invoke();
    }

    public void SetDead()
    {
        Locomotion = LocomotionState.Dead;
        Action = ActionState.None;
        OnStateChanged?.Invoke();
    }

    public bool isParryWindowActive()
    {
        return Action == ActionState.Defending &&
               Time.time - defendStartTime <= parryWindow;
    }

    public void LockInput()
    {
        InputLocked = true;
    }

    public void UnlockInput()
    {
        InputLocked = false;
    }

}
