using System.Collections.Generic;
using UnityEngine;

public class RandomLake : MonoBehaviour
{
    public GameObject[] lakePrefabs;
    public int spawnCount = 1;
    public float minDistance = 3f;
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

        while (spawned < spawnCount && attempt < spawnCount * 20)
        {
            attempt++;

            Vector2 randomPoint = GetRandomPointInBox();

            if (IsFarEnough(randomPoint) && !IsOnBlockedLayer(randomPoint))
            {
                GameObject prefab = lakePrefabs[Random.Range(0, lakePrefabs.Length)];
                Instantiate(prefab, randomPoint, Quaternion.identity, transform);

                spawnedPositions.Add(randomPoint);
                spawned++;
            }
        }
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
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);
        foreach (Collider2D hit in hits)
        {
            if (((1 << hit.gameObject.layer) & blockedLayers) != 0)
                return true;
        }
        return false;
    }
}
