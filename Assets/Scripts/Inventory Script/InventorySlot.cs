using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image background;
    public Image icon;
    public TMP_Text amountText;

    public Sprite normalSprite;
    public Sprite selectedSprite;

    InventoryItem item;

    void Awake()
    {
        Clear();
        SetSelected(false);
    }

    public void Setup(InventoryItem newItem)
    {
        item = newItem;

        if (item.item is SeedItem seed)
            icon.sprite = seed.icon;
        else if (item.item is CropsItem crop)
            icon.sprite = crop.icon;
        else
            icon.sprite = null;

        icon.enabled = icon.sprite != null;

        amountText.text = item.amount.ToString();
        amountText.enabled = true;
    }

    public void Clear()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        amountText.text = "";
        amountText.enabled = false;
    }

    public void SetSelected(bool value)
    {
        if (background)
            background.sprite = value ? selectedSprite : normalSprite;
    }

    public InventoryItem GetItem()
    {
        return item;
    }
}
