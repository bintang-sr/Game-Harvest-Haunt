using UnityEngine;

public class SacrificeManager : MonoBehaviour
{
    public static SacrificeManager Instance;
    public int minTargetValue = 3;
    public int maxTargetValue = 10;
    public int currentValue;
    public int targetValue;
    int dayCount;
    float lastDayProgress = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (LoadContext.SelectedSlot == -1)
            ResetSacrifice();
    }

    void Update()
    {
        if (!DayCycle.Instance) return;
        float progress = DayCycle.Instance.DayProgress;
        if (progress < lastDayProgress)
            OnNewDay();
        lastDayProgress = progress;
    }

    void OnNewDay()
    {
        dayCount++;
        if (dayCount < 3)
            return;
        dayCount = 0;
        if (IsRequirementMet())
        {
            if (PlayerHealth.Instance)
                PlayerHealth.Instance.Heal(1);
            ResetSacrifice();
        }
        else
        {
            if (PlayerHealth.Instance)
                PlayerHealth.Instance.TakeSacrificeDamage(1);
            ResetSacrifice();
        }
    }

    public void AddValue(int value)
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, targetValue);
    }

    public bool IsRequirementMet()
    {
        return targetValue > 0 && currentValue >= targetValue;
    }

    void ResetSacrifice()
    {
        currentValue = 0;
        targetValue = Random.Range(minTargetValue, maxTargetValue + 1);
    }
}
