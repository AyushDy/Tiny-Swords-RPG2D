using Unity.VisualScripting;
using UnityEngine;

public class Player_inputHandler : MonoBehaviour
{
    public Player_state state;
    public Player_combat combat;
    public Player_Bow bow;
    public Player_dash dash;




    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Dash"))
        {
            dash.TryDash();
        }

        if(Input.GetButtonDown("Slash"))
        {
            if(state.Equipment == EquipmentState.Bow)
            {
                bow.StartShooting();
                return;
            }
            combat.Attack();
        }


        //Equipment change
        if (Input.GetButtonDown("ChangeEquipment"))
        {
            if (state.Equipment == EquipmentState.Sword)
            {
                state.SetEquipment(EquipmentState.Bow);
                state.TrySetLocomotion(LocomotionState.Aiming);
            }
            else
            {
                state.SetEquipment(EquipmentState.Sword);
                state.TrySetLocomotion(LocomotionState.Normal, true);
            }
        }

        //defend
        if (Input.GetKeyDown(KeyCode.LeftShift) && state.Equipment == EquipmentState.Sword)
        {
            Debug.Log("Defend button pressed");
            state.TryStartAction(ActionState.Defending, 999f);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (state.Action == ActionState.Defending)
                state.EndAction();
        }
    }
}
