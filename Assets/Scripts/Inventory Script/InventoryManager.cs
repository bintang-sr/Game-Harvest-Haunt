using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxStack = 99;
    public int maxSlot = 5;
    public InventoryUI inventoryUI;
    public int selectedIndex = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (LoadContext.SelectedSlot == -1)
        {
            ClearInventory();
        }
    }

    public InventoryItem GetSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count)
            return null;

        return items[selectedIndex];
    }

    public bool HasItem(ScriptableObject item, int amount = 1)
    {
        InventoryItem existing = items.Find(i => i.item == item);
        return existing != null && existing.amount >= amount;
    }

    public bool AddItem(ScriptableObject item, int amount = 1)
    {
        InventoryItem existing = items.Find(i => i.item == item);

        if (existing != null)
        {
            existing.amount = Mathf.Min(existing.amount + amount, maxStack);
            RefreshUI();
            return true;
        }

        if (items.Count >= maxSlot)
            return false;

        int finalAmount = Mathf.Clamp(amount, 1, maxStack);
        items.Add(new InventoryItem(item, finalAmount));
        RefreshUI();
        return true;
    }

    public bool RemoveItem(InventoryItem target, int amount = 1)
    {
        if (target == null || target.amount < amount)
            return false;

        target.amount -= amount;

        if (target.amount <= 0)
        {
            int removedIndex = items.IndexOf(target);
            items.Remove(target);

            if (selectedIndex == removedIndex)
                selectedIndex = -1;
            else if (selectedIndex > removedIndex)
                selectedIndex--;
        }

        RefreshUI();
        return true;
    }

    public bool RemoveItem(ScriptableObject item, int amount = 1)
    {
        InventoryItem existing = items.Find(i => i.item == item);
        if (existing == null) return false;
        return RemoveItem(existing, amount);
    }

    public void ClearInventory()
    {
        items.Clear();
        selectedIndex = -1;
        RefreshUI();
    }

    public void ResetInventoryByJumpscare()
    {
        ClearInventory();
        Debug.Log("Inventory reset by jumpscare");
    }

    void RefreshUI()
    {
        if (inventoryUI)
            inventoryUI.Refresh();
    }
}
