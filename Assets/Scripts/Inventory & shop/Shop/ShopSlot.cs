using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image ItemImage;


    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopInfo shopInfo;
    public int price;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (shopManager == null)
        {
            shopManager = GetComponentInParent<ShopManager>();
        }

        if (shopInfo == null)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                shopInfo = parentCanvas.GetComponentInChildren<ShopInfo>(true);
            }
        }
    }

    public void Initialize(ItemSO newItemSO, int price)
    {
        ResolveReferences();
        itemSO = newItemSO;
        ItemImage.sprite = itemSO.icon;
        itemNameText.text = itemSO.itemName;
        this.price = price;
        priceText.text = price.ToString();
    }


    public void OnBuyButtonClicked()
    {
        ResolveReferences();
        if (shopManager == null || itemSO == null)
        {
            return;
        }

        shopManager.TryBuyItem(itemSO, price);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ResolveReferences();
        if (itemSO != null && shopInfo != null)
            shopInfo.ShowItemInfo(itemSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shopInfo != null)
        {
            shopInfo.HideItemInfo();
        }
    }
}
