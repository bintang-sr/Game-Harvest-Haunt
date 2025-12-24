using UnityEngine;


[ExecuteAlways] 
public class ItemPickup : MonoBehaviour
{
    public ScriptableObject itemData;
    public Animator animator;
    public string pickupTrigger = "PickUp";

    bool pickedUp = false;
    SpriteRenderer spriteRenderer;

    void OnValidate()
    {
        UpdateSprite();
    }

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (itemData == null) return;
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        var iconProp = itemData.GetType().GetProperty("icon");
        if (iconProp != null)
        {
            spriteRenderer.sprite = iconProp.GetValue(itemData) as Sprite;
            return;
        }

        var iconField = itemData.GetType().GetField("icon");
        if (iconField != null)
        {
            spriteRenderer.sprite = iconField.GetValue(itemData) as Sprite;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp) return;
        if (other.CompareTag("Player"))
        {
            TryPickUp();
        }
    }

    void TryPickUp()
    {
        bool added = InventoryManager.Instance.AddItem(itemData, 1);

        if (added)
        {
            pickedUp = true;

            if (animator)
                animator.SetTrigger(pickupTrigger);
        }
        else
        {
            pickedUp = false;
            Debug.Log("Inventory Full!");
        }
    }

    public void OnPickupAnimationEnd()
    {
        Destroy(gameObject);
    }
}
