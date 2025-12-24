using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventorySlot[] slots;
    public int selectedIndex = 0;

    void Start()
    {
        SelectSlot(0);
        Refresh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        selectedIndex = index;
        InventoryManager.Instance.selectedIndex = index;

        for (int i = 0; i < slots.Length; i++)
            slots[i].SetSelected(i == selectedIndex);

        UpdatePlayerHeldItem();
    }

    void UpdatePlayerHeldItem()
    {
        PlayerPlant player = FindObjectOfType<PlayerPlant>();
        if (!player) return;

        InventoryItem item = InventoryManager.Instance.GetSelectedItem();
        if (item != null && item.item is SeedItem seed)
            player.SetSeed(seed);
        else
            player.SetSeed(null);
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Clear();

        var items = InventoryManager.Instance.items;

        for (int i = 0; i < items.Count && i < slots.Length; i++)
            slots[i].Setup(items[i]);

        SelectSlot(selectedIndex);
    }
}
