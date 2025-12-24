using System.Collections.Generic;
using UnityEngine;

public class RandomTree : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public GameObject[] bushPrefabs;
    public float treePercentage = 0.7f;
    public int spawnCount = 30;
    public float minDistance = 1.5f;
    public LayerMask blockedLayers;

    BoxCollider2D area;
    List<Vector2> spawnedPositions = new List<Vector2>();

    void Awake()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void Spawn()
    {
        spawnedPositions.Clear();

        int spawned = 0;
        int attempt = 0;
        int maxAttempt = spawnCount * 30;

        while (spawned < spawnCount && attempt < maxAttempt)
        {
            attempt++;

            Vector2 randomPoint = GetRandomPointInBox();

            if (!IsFarEnough(randomPoint))
                continue;

            if (IsOnBlockedLayer(randomPoint))
                continue;

            GameObject prefab;

            if (Random.value < treePercentage && treePrefabs.Length > 0)
            {
                prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            }
            else if (bushPrefabs.Length > 0)
            {
                prefab = bushPrefabs[Random.Range(0, bushPrefabs.Length)];
            }
            else
            {
                continue;
            }

            Instantiate(prefab, randomPoint, Quaternion.identity, transform);
            spawnedPositions.Add(randomPoint);
            spawned++;
        }

        Debug.Log($"[RandomTree] {spawned}/{spawnCount}");
    }

    Vector2 GetRandomPointInBox()
    {
        Bounds b = area.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }

    bool IsFarEnough(Vector2 pos)
    {
        foreach (Vector2 p in spawnedPositions)
        {
            if (Vector2.Distance(p, pos) < minDistance)
                return false;
        }
        return true;
    }

    bool IsOnBlockedLayer(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos, blockedLayers);
        return hit != null;
    }
}
