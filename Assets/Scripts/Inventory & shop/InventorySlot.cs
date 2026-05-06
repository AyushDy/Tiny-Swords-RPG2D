using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemSO itemSO;
    public int quantity;

    public Image itemImage;
    public TMP_Text quantityText;


    private InventoryManager inventoryManager;
    private static ShopManager activeShop;


    private void OnEnable()
    {
        ShopKeeper.OnShopStateChange += HandleShopStateChange;
    }

    private void OnDisable()
    {
        ShopKeeper.OnShopStateChange -= HandleShopStateChange;
    }

    private void HandleShopStateChange(ShopManager shop, bool isOpen)
    {
        activeShop = isOpen ? shop : null;
    }


    private void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (quantity > 0)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (activeShop != null)
                {
                    activeShop.SellItem(itemSO);
                    quantity--;
                    UpdateUI();
                }
                else
                {
                    inventoryManager.UseItem(this);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.DropItem(this);
            }
        }
    }

    public void UpdateUI()
    {
        if(quantity <= 0)
        {
            itemSO = null;
        }
        if (itemSO != null)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = itemSO.icon;
            quantityText.text = quantity.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = string.Empty;
        }
    }

}
