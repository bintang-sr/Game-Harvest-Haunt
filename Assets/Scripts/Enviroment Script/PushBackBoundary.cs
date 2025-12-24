using UnityEngine;
using Cinemachine;

public class PushBackBoundary : MonoBehaviour
{
    public float pushForce = 25f;
    public float damping = 12f;
    public float shakeForce = 1.2f;
    public float shakeCooldown = 0.25f;

    CinemachineImpulseSource impulse;
    float shakeTimer;

    void Awake()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (!rb) return;

        Bounds b = GetComponent<BoxCollider2D>().bounds;
        Vector2 p = rb.position;

        Vector2 dir = Vector2.zero;

        if (p.x < b.min.x) dir.x = 1;
        else if (p.x > b.max.x) dir.x = -1;

        if (p.y < b.min.y) dir.y = 1;
        else if (p.y > b.max.y) dir.y = -1;

        if (dir == Vector2.zero) return;

        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, damping * Time.deltaTime);
        rb.AddForce(dir.normalized * pushForce * Time.deltaTime, ForceMode2D.Force);

        if (impulse && shakeTimer <= 0f)
        {
            impulse.GenerateImpulseWithForce(shakeForce);
            shakeTimer = shakeCooldown;
        }
    }

    void Update()
    {
        if (shakeTimer > 0f)
            shakeTimer -= Time.deltaTime;
    }
}
