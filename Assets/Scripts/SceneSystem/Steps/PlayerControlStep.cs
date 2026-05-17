using System.Collections;
using UnityEngine;


[System.Serializable]
public class PlayerControlStep : SceneStep
{
    public bool enableControl;

    public override IEnumerator Execute(SceneDirector director)
    {
        Player_state state = Object.FindAnyObjectByType<Player_state>();

        if(state != null)
        {
            if (enableControl)
            {
                state.UnlockInput();
            }
            else
            {
                state.LockInput();
            }
        }
        yield return null;
    }
}
