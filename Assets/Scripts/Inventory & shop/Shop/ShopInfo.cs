using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopInfo : MonoBehaviour
{
    public CanvasGroup infoPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemIcon;

    [Header("Effect Fields")]
    public TMP_Text[] effectTexts;

    private RectTransform infoPanelRect;

    private void Awake()
    {
        infoPanelRect = infoPanel.GetComponent<RectTransform>();
        infoPanel.interactable = false;
        infoPanel.blocksRaycasts = false;
    }

    public void ShowItemInfo(ItemSO itemSo)
    {
        infoPanel.alpha = 1;
        itemNameText.text = itemSo.itemName;
        itemDescriptionText.text = itemSo.description;
        itemIcon.sprite = itemSo.icon;
        
        effectTexts[0].text = "Effects coming soon!";
    }

    public void HideItemInfo()
    {
        infoPanel.alpha = 0;
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        
    }

    public void FollowCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 offset = new Vector3(100, -10, 0);

        infoPanelRect.position = mousePos + offset;
    }
}
