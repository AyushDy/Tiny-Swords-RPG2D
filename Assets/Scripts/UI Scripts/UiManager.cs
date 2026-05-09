using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    private bool isMenuActive;

    [SerializeField] private CanvasGroup inventoryPanel;
    [SerializeField] private CanvasGroup StatsMenu;
    [SerializeField] private CanvasGroup SkillsMenu;
    [SerializeField] private CanvasGroup QuestMenu;


    [SerializeField] private Image menuToggleImage;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;


    public void ToggleMenu(CanvasGroup menu)
    {
        SetMenuState(inventoryPanel, false);
        SetMenuState(StatsMenu, false);
        SetMenuState(SkillsMenu, false);
        SetMenuState(QuestMenu, false);

        SetMenuState(menu, true);
    }

    public void ToggleMainMenu()
    {
        isMenuActive = !isMenuActive;
        SetMenuState(menuBar, isMenuActive);
        menuToggleImage.sprite = isMenuActive ? closeSprite : openSprite;

        SetMenuState(inventoryPanel, false);
        SetMenuState(StatsMenu, false);
        SetMenuState(SkillsMenu, false);
        SetMenuState(QuestMenu, false);
    }


    private void SetMenuState(CanvasGroup menu, bool isActive)
    {
        menu.alpha = isActive ? 1 : 0;
        menu.interactable = isActive;
        menu.blocksRaycasts = isActive;
    }
}