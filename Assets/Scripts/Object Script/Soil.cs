using System.Collections;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public SpriteRenderer soilRenderer;
    public SpriteRenderer plantedRenderer;
    public SpriteRenderer growingRenderer;
    public Sprite soilWateredSprite;
    public SpriteRenderer dryOverlay;
    public SpriteRenderer wetOverlay;
    private Sprite soilDrySprite;
    private SeedItem currentSeed;
    public Animator wateringAnimator;
    private int currentDay;
    private int growDays;
    private bool isPlanted;
    private bool isReadyToHarvest;
    private bool isWateredToday;
    private float lastDayProgress;

    void Start()
    {
        soilDrySprite = soilRenderer.sprite;
        dryOverlay.enabled = true;
        wetOverlay.enabled = false;
        plantedRenderer.enabled = false;
        growingRenderer.enabled = false;
        lastDayProgress = DayCycle.Instance.DayProgress;
    }

    void Update()
    {
        float currentProgress = DayCycle.Instance.DayProgress;
        if (currentProgress < lastDayProgress)
            OnNewDay();
        lastDayProgress = currentProgress;
    }

    public void Plant(SeedItem seed)
    {
        if (isPlanted || seed == null) return;

        currentSeed = seed;
        growDays = Mathf.CeilToInt(seed.growTime);
        currentDay = 0;
        isPlanted = true;
        isReadyToHarvest = false;
        isWateredToday = false;

        dryOverlay.enabled = true;
        wetOverlay.enabled = false;

        soilRenderer.sprite = soilDrySprite;
        plantedRenderer.enabled = true;
        growingRenderer.enabled = false;
    }

    public void Water()
    {
        if (!isPlanted || isWateredToday || isReadyToHarvest) return;

        isWateredToday = true;

        dryOverlay.enabled = false;
        wetOverlay.enabled = true;

        if (wateringAnimator != null)
            wateringAnimator.SetTrigger("Water");

        soilRenderer.sprite = soilWateredSprite;
    }

    void OnNewDay()
    {
        wetOverlay.enabled = false;
        dryOverlay.enabled = true;

        soilRenderer.sprite = soilDrySprite;

        if (isPlanted && isWateredToday && !isReadyToHarvest)
        {
            currentDay++;

            if (currentDay == 1)
            {
                plantedRenderer.enabled = false;
                growingRenderer.enabled = true;
            }
            else if (currentDay >= growDays)
            {
                isReadyToHarvest = true;
                growingRenderer.sprite = currentSeed.fullgrown;
                growingRenderer.enabled = true;
                plantedRenderer.enabled = false;
            }
        }

        isWateredToday = false;
    }

    public CropsItem Harvest()
    {
        if (!isReadyToHarvest) return null;

        CropsItem crop = currentSeed.resultCrops;
        ResetSoil();
        crop.GenerateValue();
        return crop;
    }

    public void ResetSoil()
    {
        currentSeed = null;
        currentDay = 0;
        growDays = 0;
        isPlanted = false;
        isReadyToHarvest = false;
        isWateredToday = false;

        wetOverlay.enabled = false;
        dryOverlay.enabled = true;

        soilRenderer.sprite = soilDrySprite;
        plantedRenderer.enabled = false;
        growingRenderer.enabled = false;
    }

    public void LoadSoil(SeedItem seed, int currentDay, int growDays, bool isReady, bool watered)
    {
        this.currentSeed = seed;
        this.currentDay = currentDay;
        this.growDays = growDays;
        this.isPlanted = true;
        this.isReadyToHarvest = isReady;
        this.isWateredToday = watered;

        dryOverlay.enabled = !watered;
        wetOverlay.enabled = watered;

        if (isReady)
        {
            plantedRenderer.enabled = false;
            growingRenderer.sprite = seed.fullgrown;
            growingRenderer.enabled = true;
        }
        else if (currentDay > 0)
        {
            plantedRenderer.enabled = false;
            growingRenderer.enabled = true;
        }
        else
        {
            plantedRenderer.enabled = true;
            growingRenderer.enabled = false;
        }

        soilRenderer.sprite = isWateredToday ? soilWateredSprite : soilDrySprite;
    }

    public bool IsPlanted() => isPlanted;
    public bool IsReadyToHarvest() => isReadyToHarvest;
    public string GetCurrentSeedName() => currentSeed != null ? currentSeed.name : "";
    public int GetCurrentDay() => currentDay;
    public int GetGrowDays() => growDays;
    public bool IsWateredToday() => isWateredToday;
}
