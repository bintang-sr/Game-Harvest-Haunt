using UnityEngine;

public class CampBlocker : MonoBehaviour
{
    public float pushForce = 8f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Monster")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (!rb) return;

        Vector2 pushDir =
            ((Vector2)other.transform.position - (Vector2)transform.position).normalized;

        rb.velocity = Vector2.zero;
        rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
    }
}
