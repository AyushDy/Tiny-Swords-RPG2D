using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public int gold;
    public TMP_Text goldText;
    public GameObject lootPrefab;
    public Transform player;

    private void OnEnable()
    {
        Loot.OnLootPickup += AddToInventory;
    }

    private void OnDisable()
    {
        Loot.OnLootPickup -= AddToInventory;
    }

    public void AddToInventory(ItemSO itemSO, int quantity)
    {
        if (itemSO.isGold)
        {
            gold += quantity;
            goldText.text = gold.ToString();
            return;
        }
        foreach (var slot in inventorySlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
            {
                int availableSpace = itemSO.stackSize - slot.quantity;
                int amountToAdd = Mathf.Min(quantity, availableSpace);

                slot.quantity += amountToAdd;
                quantity -= amountToAdd;

                slot.UpdateUI();

                if (quantity <= 0)
                    return;
            }
        }


        foreach (var slot in inventorySlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(quantity, itemSO.stackSize);
                slot.itemSO = itemSO;
                slot.quantity = amountToAdd;
                quantity -= amountToAdd;
                slot.UpdateUI();

                if (quantity <= 0)
                    return;
            }


        }

        if (quantity > 0)
        {
            DropLoot(itemSO, quantity);
        }
    }

    public void DropItem(InventorySlot slot)
    {
        if (slot.itemSO != null)
        {
            DropLoot(slot.itemSO, 1);
            slot.quantity -= 1;
            if (slot.quantity <= 0){
                slot.itemSO = null;
            }
            slot.UpdateUI();
        }
    }

    private void DropLoot(ItemSO itemSO, int quantity)
    {
        Loot loot = Instantiate(lootPrefab, player.position, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(itemSO, quantity);
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.itemSO != null)
        {
            Debug.Log($"Used {slot.itemSO.itemName}");
        }
    }
}