using UnityEngine;
using System.Collections;

public class PlayerPlant : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(0.8f, 0.8f);
    public float boxDistance = 0.6f;
    public LayerMask soilLayer;

    public SeedItem currentSeed;
    public float lockMovementTime = 1f;

    bool hasInteractedThisPress = false;
    bool isSquatting = false;

    Animator animator;
    PlayerMovement playerMovement;
    Rigidbody2D rb;

    Vector2 lastDirection = Vector2.right;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isSquatting) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
            lastDirection = new Vector2(h, v).normalized;

        if (Input.GetKey(KeyCode.E))
        {
            if (!hasInteractedThisPress)
            {
                TryPlant();
                hasInteractedThisPress = true;
            }
        }
        else
        {
            hasInteractedThisPress = false;
        }
    }

    public void SetSeed(SeedItem seed)
    {
        currentSeed = seed;
    }

    void TryPlant()
    {
        Vector2 origin = (Vector2)transform.position + lastDirection * boxDistance;

        Collider2D[] hits = Physics2D.OverlapBoxAll(origin, boxSize, 0f, soilLayer);
        if (hits.Length == 0) return;

        Soil targetSoil = null;
        float closest = float.MaxValue;

        foreach (var h in hits)
        {
            Soil s = h.GetComponent<Soil>();
            if (!s) continue;

            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d < closest)
            {
                closest = d;
                targetSoil = s;
            }
        }

        if (targetSoil == null) return;

        if (!targetSoil.IsPlanted())
        {
            if (currentSeed == null) return;
            if (!InventoryManager.Instance) return;
            if (!InventoryManager.Instance.HasItem(currentSeed)) return;

            ForceStopMovement();

            targetSoil.Plant(currentSeed);

            InventoryManager.Instance.RemoveItem(currentSeed, 1);
            if (!InventoryManager.Instance.HasItem(currentSeed))
                currentSeed = null;

            StartCoroutine(SquatRoutine());
            return;
        }

        if (targetSoil.IsReadyToHarvest())
        {
            CropsItem crop = targetSoil.Harvest();
            if (crop != null)
                InventoryManager.Instance.AddItem(crop, 1);
        }
    }

    void ForceStopMovement()
    {
        if (rb)
            rb.velocity = Vector2.zero;

        if (playerMovement)
            playerMovement.enabled = false;
    }

    IEnumerator SquatRoutine()
    {
        isSquatting = true;

        animator.SetBool("IsSquatting", true);

        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animLength = stateInfo.length;

        yield return new WaitForSeconds(lockMovementTime);
        yield return new WaitForSeconds(animLength - lockMovementTime);

        animator.SetBool("IsSquatting", false);

        if (playerMovement)
            playerMovement.enabled = true;

        isSquatting = false;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 dir = Application.isPlaying ? lastDirection : Vector2.right;
        Vector2 origin = (Vector2)transform.position + dir * boxDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(origin, boxSize);
    }
}
