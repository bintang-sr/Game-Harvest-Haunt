using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySaveData
{
    public string itemName;
    public int amount;
}

[System.Serializable]
public class SoilSaveData
{
    public bool isPlanted;
    public string seedName;
    public int currentDay;
    public int growDays;
    public bool isReadyToHarvest;
    public bool isWateredToday;
}

[System.Serializable]
public class GameSaveData
{
    public float playerX, playerY, playerZ;
    public int playerHP;
    public int currentWater;
    public List<InventorySaveData> inventoryItems = new List<InventorySaveData>();
    public List<SoilSaveData> soils = new List<SoilSaveData>();
    public int sacrificeCurrentValue;
    public int sacrificeTargetValue;
    public int currentDay;
    public float playTime;
}

public class GameSaveSystem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerHealth playerHealth;
    public PlayerWater playerWater;
    public Soil[] allSoils;
    public SacrificeManager sacrificeManager;

    private float loadedPlayTime = 0f;

    private string GetSlotKey(int slot) => $"saveData_slot{slot}";

    void Start()
    {
        if (LoadContext.SelectedSlot != -1)
        {
            LoadGame(LoadContext.SelectedSlot);
            LoadContext.SelectedSlot = -1;
        }
    }

    public void SaveGame(int slot)
    {
        GameSaveData saveData = new GameSaveData();

        saveData.playerX = playerHealth.transform.position.x;
        saveData.playerY = playerHealth.transform.position.y;
        saveData.playerZ = playerHealth.transform.position.z;

        saveData.playerHP = playerHealth.hp;
        saveData.currentWater = playerWater.GetCurrentWater();

        saveData.inventoryItems.Clear();
        foreach (var item in inventoryManager.items)
        {
            saveData.inventoryItems.Add(new InventorySaveData
            {
                itemName = item.item.name,
                amount = item.amount
            });
        }

        saveData.soils.Clear();
        foreach (var soil in allSoils)
        {
            SoilSaveData sData = new SoilSaveData
            {
                isPlanted = soil.IsPlanted(),
                seedName = soil.IsPlanted() ? soil.GetCurrentSeedName() : "",
                currentDay = soil.GetCurrentDay(),
                growDays = soil.GetGrowDays(),
                isReadyToHarvest = soil.IsReadyToHarvest(),
                isWateredToday = soil.IsWateredToday()
            };
            saveData.soils.Add(sData);
        }

        saveData.sacrificeCurrentValue = sacrificeManager.currentValue;
        saveData.sacrificeTargetValue = sacrificeManager.targetValue;

        saveData.currentDay = DayCycle.Instance.CurrentDay;
        saveData.playTime = loadedPlayTime + Time.timeSinceLevelLoad;

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(GetSlotKey(slot), json);
        PlayerPrefs.Save();
    }

    public void LoadGame(int slot)
    {
        string key = GetSlotKey(slot);
        if (!PlayerPrefs.HasKey(key)) return;

        string json = PlayerPrefs.GetString(key);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        loadedPlayTime = saveData.playTime;

        playerHealth.transform.position = new Vector3(
            saveData.playerX,
            saveData.playerY,
            saveData.playerZ
        );

        playerHealth.hp = saveData.playerHP;
        playerWater.SetCurrentWater(saveData.currentWater);

        inventoryManager.ClearInventory();
        foreach (var itemData in saveData.inventoryItems)
        {
            ScriptableObject itemSO = Resources.Load<ScriptableObject>("SeedItems/" + itemData.itemName);
            if (itemSO == null)
                itemSO = Resources.Load<ScriptableObject>("CropItems/" + itemData.itemName);

            if (itemSO != null)
                inventoryManager.AddItem(itemSO, itemData.amount);
            else
                Debug.LogWarning("Item not found in Resources: " + itemData.itemName);
        }

        for (int i = 0; i < allSoils.Length; i++)
        {
            if (i >= saveData.soils.Count) 
            {
                allSoils[i].ResetSoil();
                continue;
            }

            var sData = saveData.soils[i];

            if (sData.isPlanted)
            {
                SeedItem seedSO = Resources.Load<SeedItem>("SeedItems/" + sData.seedName);
                if (seedSO != null)
                {
                    allSoils[i].LoadSoil(
                        seedSO,
                        sData.currentDay,
                        sData.growDays,
                        sData.isReadyToHarvest,
                        sData.isWateredToday
                    );
                }
                else
                {
                    Debug.LogWarning("Seed not found in Resources: " + sData.seedName);
                    allSoils[i].ResetSoil();
                }
            }
            else
            {
                allSoils[i].ResetSoil();
            }
        }

        sacrificeManager.currentValue = saveData.sacrificeCurrentValue;
        sacrificeManager.targetValue = saveData.sacrificeTargetValue;

        DayCycle.Instance.SetDay(saveData.currentDay);
    }

    public bool HasSave(int slot)
    {
        return PlayerPrefs.HasKey(GetSlotKey(slot));
    }

    public GameSaveData GetSaveData(int slot)
    {
        string key = GetSlotKey(slot);
        if (!PlayerPrefs.HasKey(key)) return null;

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<GameSaveData>(json);
    }
}
