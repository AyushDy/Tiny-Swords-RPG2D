using UnityEngine;
using UnityEngine.UI;

public class ShopButtonToggles : MonoBehaviour
{
    [SerializeField] private Button[] sectionButtons;

    private void Awake()
    {
        AutoWireButtons();
    }

    private void AutoWireButtons()
    {
        if (sectionButtons == null ||
            sectionButtons.Length < 3 ||
            sectionButtons[0] == null ||
            sectionButtons[1] == null ||
            sectionButtons[2] == null)
        {
            sectionButtons = GetComponentsInChildren<Button>(true);
        }

        if (sectionButtons == null || sectionButtons.Length < 3)
        {
            return;
        }

        WireButton(sectionButtons[0], OpenItemShop);
        WireButton(sectionButtons[1], OpenWeaponShop);
        WireButton(sectionButtons[2], OpenArmorShop);
    }

    private static void WireButton(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null || button.onClick.GetPersistentEventCount() > 0)
        {
            return;
        }

        button.onClick.AddListener(action);
    }

    public void OpenItemShop()
    {
        if(ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenItemShop();
        }
    }

    public void OpenWeaponShop()
    {
        if(ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenWeaponShop();
        }
    }

    public void OpenArmorShop()
    {
        if(ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenArmorShop();
        }
    }
}
