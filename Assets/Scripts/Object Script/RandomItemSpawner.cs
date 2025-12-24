using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    public GameObject[] seedPrefabs;
    public GameObject[] cropPrefabs;
    [Range(0f, 1f)] public float seedPercentage = 0.9f;
    [Range(0f, 1f)] public float cropPercentage = 0.1f;
    public int spawnCount = 1000;
    public float minDistance = 1.5f;
    public LayerMask blockedLayers;

    public int respawnEveryDays = 3;

    BoxCollider2D area;
    List<Vector2> spawnedPositions = new List<Vector2>();
    Collider2D[] hits = new Collider2D[1];

    int lastSpawnDay = -1;

    void Start()
    {
        area = GetComponent<BoxCollider2D>();

        if (DayCycle.Instance != null)
            lastSpawnDay = DayCycle.Instance.CurrentDay;

        Spawn();
    }

    void Update()
    {
        if (DayCycle.Instance == null) return;

        int currentDay = DayCycle.Instance.CurrentDay;

        if (currentDay - lastSpawnDay >= respawnEveryDays)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i).gameObject);

            Spawn();
            lastSpawnDay = currentDay;
        }
    }

    void Spawn()
    {
        spawnedPositions.Clear();

        int spawned = 0;
        int attempt = 0;
        int maxAttempt = spawnCount * 50;

        while (spawned < spawnCount && attempt < maxAttempt)
        {
            attempt++;

            Vector2 p = RandomPoint();

            if (!FarEnough(p)) continue;
            if (Physics2D.OverlapCircleNonAlloc(p, 0.1f, hits, blockedLayers) > 0) continue;

            GameObject prefab = Pick();
            if (prefab == null) continue;

            Instantiate(prefab, new Vector3(p.x, p.y, 0f), Quaternion.identity, transform);
            spawnedPositions.Add(p);
            spawned++;
        }
    }

    Vector2 RandomPoint()
    {
        Bounds b = area.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }

    bool FarEnough(Vector2 p)
    {
        for (int i = 0; i < spawnedPositions.Count; i++)
            if (Vector2.Distance(spawnedPositions[i], p) < minDistance)
                return false;
        return true;
    }

    GameObject Pick()
    {
        float r = Random.value;
        if (r < seedPercentage && seedPrefabs.Length > 0)
            return seedPrefabs[Random.Range(0, seedPrefabs.Length)];
        if (r < seedPercentage + cropPercentage && cropPrefabs.Length > 0)
            return cropPrefabs[Random.Range(0, cropPrefabs.Length)];
        return null;
    }
}
