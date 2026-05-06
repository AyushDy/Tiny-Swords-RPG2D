using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    bool inventoryOpen;
    public CanvasGroup inventoryCanvas;
    void Update()
    {
        if (Input.GetButtonDown("ToggleInventory"))
        {
            if(inventoryOpen)
            {
                Time.timeScale = 1;
                inventoryCanvas.alpha = 0;
                inventoryCanvas.blocksRaycasts = false;
                inventoryOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                inventoryCanvas.alpha = 1;
                inventoryCanvas.blocksRaycasts = true;
                inventoryOpen = true;
            }
        }
    }
}
