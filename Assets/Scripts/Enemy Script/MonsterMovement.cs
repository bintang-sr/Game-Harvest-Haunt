using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform player;
    public Collider2D wanderArea;

    public float wanderSpeed = 1.2f;
    public float chaseSpeed = 2.5f;
    public float aggressiveSpeed = 4.5f;
    public float acceleration = 6f;

    public float minTargetDistance = 1.2f;
    public float checkDistance = 0.7f;

    public float orbitSpeed = 1.5f;
    public float campBuffer = 0.2f;
    public float maxOrbitTime = 3f;

    public float wanderChangeTime = 2.5f;

    public LayerMask obstacleLayer;

    Rigidbody2D rb;
    Collider2D self;
    SpriteRenderer sr;

    Vector2 wanderTarget;
    Vector2 velocity;

    bool isChasing;
    bool orbitingCamp;
    float orbitTimer;

    float wanderTimer;

    int campLayer;
    Collider2D currentCamp;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        self = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        campLayer = LayerMask.NameToLayer("Camp");

        PickTarget();
    }

    public void SetChase(bool value, bool forceStop)
    {
        if (forceStop)
        {
            isChasing = false;
            velocity = Vector2.zero;
            orbitingCamp = false;
            currentCamp = null;
            return;
        }

        isChasing = value;
    }

    public void UpdateChaseSpeed(int hp)
    {
        if (hp <= 2)
            chaseSpeed = aggressiveSpeed;
        else
            chaseSpeed = wanderSpeed;
    }

    void FixedUpdate()
    {
        if (!PlayerHealth.Instance || PlayerHealth.Instance.hp <= 0) return;

        ForceOutOfCamp();

        if (orbitingCamp && currentCamp)
        {
            OrbitCamp();
            FlipSprite(velocity.x);
            return;
        }

        Vector2 target = isChasing && player ? player.position : wanderTarget;
        float speed = isChasing ? chaseSpeed : wanderSpeed;

        Vector2 dir = target - rb.position;

        if (!isChasing)
        {
            wanderTimer += Time.fixedDeltaTime;

            if (wanderTimer >= wanderChangeTime || dir.magnitude < 0.4f)
            {
                PickTarget();
                wanderTimer = 0f;
            }
        }
        else
        {
            wanderTimer = 0f;
        }

        if (dir == Vector2.zero)
            dir = Random.insideUnitCircle.normalized;
        else
            dir.Normalize();

        if (Blocked(dir, out Collider2D camp))
        {
            if (camp)
            {
                StartOrbit(camp);
                return;
            }

            Vector2 side = Vector2.Perpendicular(dir);

            if (!Blocked(side, out _)) dir = side;
            else if (!Blocked(-side, out _)) dir = -side;
        }

        velocity = Vector2.MoveTowards(
            velocity,
            dir * speed,
            acceleration * Time.fixedDeltaTime
        );

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        FlipSprite(velocity.x);
    }

    void FlipSprite(float x)
    {
        if (x > 0.01f) sr.flipX = true;
        else if (x < -0.01f) sr.flipX = false;
    }

    void PickTarget()
    {
        Bounds b = wanderArea.bounds;

        for (int i = 0; i < 30; i++)
        {
            Vector2 p = new Vector2(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y)
            );

            if (Vector2.Distance(p, rb.position) < minTargetDistance)
                continue;

            wanderTarget = p;
            return;
        }

        wanderTarget = rb.position + Random.insideUnitCircle.normalized;
    }

    bool Blocked(Vector2 dir, out Collider2D camp)
    {
        camp = null;

        RaycastHit2D hit = Physics2D.CircleCast(
            rb.position,
            0.25f,
            dir,
            checkDistance,
            obstacleLayer | (1 << campLayer)
        );

        if (!hit || hit.collider == self) return false;

        if (hit.collider.gameObject.layer == campLayer)
        {
            camp = hit.collider;
            return true;
        }

        return true;
    }

    void StartOrbit(Collider2D camp)
    {
        if (orbitingCamp) return;

        orbitingCamp = true;
        currentCamp = camp;
        orbitTimer = 0f;
        velocity = Vector2.zero;
    }

    void OrbitCamp()
    {
        orbitTimer += Time.fixedDeltaTime;

        if (orbitTimer > maxOrbitTime)
        {
            orbitingCamp = false;
            currentCamp = null;
            return;
        }

        Vector2 center = currentCamp.bounds.center;
        Vector2 offset = (Vector2)rb.position - center;

        if (offset == Vector2.zero)
            offset = Vector2.right;

        Vector2 tangent = new Vector2(-offset.y, offset.x).normalized;

        velocity = Vector2.MoveTowards(
            velocity,
            tangent * orbitSpeed,
            acceleration * Time.fixedDeltaTime
        );

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void ForceOutOfCamp()
    {
        Collider2D hit = Physics2D.OverlapCircle(rb.position, 0.25f, 1 << campLayer);
        if (!hit) return;

        Vector2 away = ((Vector2)rb.position - (Vector2)hit.bounds.center).normalized;
        rb.MovePosition(rb.position + away * 0.4f);
    }
}
