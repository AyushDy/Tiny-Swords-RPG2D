using UnityEngine;

public class ToggleSkillTrees : MonoBehaviour
{
    public CanvasGroup statsGroup;
    private bool skillTreeOpen = false;

    void Update()
    {
        if (Input.GetButtonDown("ToggleSkillTrees"))
        {
            if (skillTreeOpen)
            {
                Time.timeScale = 1;
                statsGroup.alpha = 0;
                statsGroup.blocksRaycasts = false;
                skillTreeOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                statsGroup.alpha = 1;
                statsGroup.blocksRaycasts = true;
                skillTreeOpen = true;
            }
        }
    }
}
