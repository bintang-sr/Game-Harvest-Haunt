using UnityEngine;

public class MonsterRespawn : MonoBehaviour
{
    public BoxCollider2D spawnArea;
    public Camera mainCamera;
    public LayerMask forbiddenLayers;

    float checkRadius = 0.4f;
    float lastDayProgress;
    bool isDespawning;

    void Start()
    {
        Respawn();
        lastDayProgress = DayCycle.Instance.DayProgress;
    }

    void Update()
    {
        if (DayCycle.Instance.DayProgress < lastDayProgress)
            OnNewDay();

        lastDayProgress = DayCycle.Instance.DayProgress;

        if (isDespawning)
            MoveAwayFromCamera();
    }

    void OnNewDay()
    {
        if (IsVisibleFromCamera())
            isDespawning = true;
        else
            Respawn();
    }

    void MoveAwayFromCamera()
    {
        Vector3 dir = (transform.position - mainCamera.transform.position).normalized;
        transform.position += dir * Time.deltaTime * 5f;

        if (!IsVisibleFromCamera())
        {
            isDespawning = false;
            Respawn();
        }
    }

    void Respawn()
    {
        Vector2 pos;
        int attempts = 0;

        do
        {
            pos = GetRandomPointInBox(spawnArea);
            attempts++;
            if (attempts > 50) break;
        }
        while (IsForbiddenArea(pos));

        transform.position = pos;
    }

    bool IsForbiddenArea(Vector2 point)
    {
        return Physics2D.OverlapCircle(point, checkRadius, forbiddenLayers);
    }

    Vector2 GetRandomPointInBox(BoxCollider2D box)
    {
        Bounds b = box.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }

    bool IsVisibleFromCamera()
    {
        Vector3 v = mainCamera.WorldToViewportPoint(transform.position);
        return v.x > 0 && v.x < 1 && v.y > 0 && v.y < 1 && v.z > 0;
    }
}
