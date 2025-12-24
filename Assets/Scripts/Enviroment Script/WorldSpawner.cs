using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public RandomLake lakeSpawner;
    public RandomTree treeSpawner;
    public RandomDetail detailSpawner;

    public int daysPerWorld = 2;

    int currentDay = 1;
    int lastWorldIndex = -1;
    float lastDayProgress;

    void Start()
    {
        GenerateWorld();
    }

    void Update()
    {
        if (DayCycle.Instance == null) return;

        if (DayCycle.Instance.DayProgress < lastDayProgress)
        {
            OnNewDay();
        }

        lastDayProgress = DayCycle.Instance.DayProgress;
    }

    void OnNewDay()
    {
        currentDay++;

        int worldIndex = (currentDay - 1) / daysPerWorld;

        if (worldIndex != lastWorldIndex)
        {
            GenerateWorld();
        }
    }

    void GenerateWorld()
    {
        int worldIndex = (currentDay - 1) / daysPerWorld;

        Random.InitState(worldIndex * 9999 + 777);

        ClearWorld();

        lakeSpawner?.Spawn(); 
        treeSpawner?.Spawn(); 
        detailSpawner?.Spawn(); 

        lastWorldIndex = worldIndex;

        Debug.Log("[WorldSpawner] World regenerated | Day " + currentDay);
    }

    void ClearWorld()
    {
        ClearChildren(lakeSpawner?.transform);
        ClearChildren(treeSpawner?.transform);
        ClearChildren(detailSpawner?.transform);
    }

    void ClearChildren(Transform parent)
    {
        if (parent == null) return;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
