using UnityEngine;
using UnityEngine.UI;

public class PlayerWater : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(0.8f, 0.8f);
    public float boxDistance = 0.6f;
    public LayerMask soilLayer;
    public LayerMask lakeLayer;

    public WateringCanItem wateringCan;

    public Image wateringCanUI;

    int currentWater;
    Vector2 lastDirection = Vector2.right;

    void Start()
    {
        if (wateringCan)
            currentWater = wateringCan.maxWater;

        UpdateUI();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
            lastDirection = new Vector2(h, v).normalized;

        if (Input.GetKeyDown(KeyCode.F))
            Water();

        if (Input.GetKeyDown(KeyCode.R))
            TryRefill();
    }

    void Water()
    {
        if (!wateringCan) return;
        if (currentWater <= 0) return;

        Vector2 origin = (Vector2)transform.position + lastDirection * boxDistance;

        Collider2D hit = Physics2D.OverlapBox(origin, boxSize, 0f, soilLayer);
        if (!hit) return;

        Soil soil = hit.GetComponent<Soil>();
        if (!soil) return;

        if (!soil.IsPlanted() || soil.IsReadyToHarvest()) return;

        soil.Water();
        currentWater--;

        UpdateUI();
    }

    void TryRefill()
    {
        if (!wateringCan) return;
        if (currentWater >= wateringCan.maxWater) return;

        Vector2 origin = (Vector2)transform.position + lastDirection * boxDistance;

        Collider2D hit = Physics2D.OverlapBox(origin, boxSize, 0f, lakeLayer);
        if (!hit) return;
        if (!hit.CompareTag("Lake")) return;

        currentWater = wateringCan.maxWater;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!wateringCanUI || !wateringCan) return;

        if (currentWater >= wateringCan.maxWater)
            wateringCanUI.sprite = wateringCan.fullSprite;
        else if (currentWater >= 1)
            wateringCanUI.sprite = wateringCan.halfSprite;
        else
            wateringCanUI.sprite = wateringCan.emptySprite;
    }

    public int GetCurrentWater()
    {
        return currentWater;
    }

    public void SetCurrentWater(int amount)
    {
        currentWater = Mathf.Clamp(amount, 0, wateringCan.maxWater);
        UpdateUI();
    }

    void OnDrawGizmosSelected()
    {
        Vector2 dir = Application.isPlaying ? lastDirection : Vector2.right;
        Vector2 origin = (Vector2)transform.position + dir * boxDistance;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(origin, boxSize);
    }
}
