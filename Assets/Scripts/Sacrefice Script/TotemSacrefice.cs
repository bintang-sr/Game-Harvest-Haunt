using UnityEngine;

public class TotemSacrifice : MonoBehaviour
{
    public SpriteRenderer indicatorRenderer;
    public Sprite notReadySprite;
    public Sprite readySprite;

    bool playerInside;

    void Update()
    {
        UpdateIndicator();

        if (playerInside && Input.GetKeyDown(KeyCode.E))
            TrySacrifice();
    }

    void TrySacrifice()
    {
        if (!SacrificeManager.Instance) return;
        if (SacrificeManager.Instance.IsRequirementMet()) return;

        InventoryItem selected = InventoryManager.Instance.GetSelectedItem();
        if (selected == null) return;
        if (!(selected.item is CropsItem)) return;

        SacrificeManager.Instance.AddValue(selected.fixedValue);
        InventoryManager.Instance.RemoveItem(selected, 1);
    }

    void UpdateIndicator()
    {
        if (!indicatorRenderer || !SacrificeManager.Instance)
            return;

        indicatorRenderer.sprite =
            SacrificeManager.Instance.IsRequirementMet()
            ? readySprite
            : notReadySprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
