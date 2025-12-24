using System.Collections.Generic;
using UnityEngine;

public class RandomDetail : MonoBehaviour
{
    public GameObject[] prefabs;
    public int spawnCount = 30;
    public float minDistance = 1.5f;
    public float maxRotation = 360f;

    public LayerMask blockedLayers;

    BoxCollider2D area;
    List<Vector2> spawnedPositions = new List<Vector2>();

    void Awake()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void Spawn()
    {
        int spawned = 0;
        int attempt = 0;

        while (spawned < spawnCount && attempt < spawnCount * 10)
        {
            attempt++;
            Vector2 randomPoint = GetRandomPointInBox();

            if (IsFarEnough(randomPoint) && !IsOnBlockedLayer(randomPoint))
            {
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                float rotationZ = Random.Range(0f, maxRotation);
                Instantiate(
                    prefab,
                    randomPoint,
                    Quaternion.Euler(0f, 0f, rotationZ),
                    transform
                );

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
        Collider2D hit = Physics2D.OverlapPoint(pos, blockedLayers);
        return hit != null;
    }
}
