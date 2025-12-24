using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static DayCycle Instance;

    public float dayDuration = 4f;
    public float nightDuration = 4f;

    float timer;
    float fullCycle;

    public int CurrentDay { get; private set; } = 1;

    public bool IsNight { get; private set; }
    public bool IsDay => !IsNight;
    public float DayProgress => timer / fullCycle;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        fullCycle = dayDuration + nightDuration;

        if (LoadContext.SelectedSlot == -1)
        {
            timer = 0f;
            IsNight = false;
            CurrentDay = 1;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fullCycle)
        {
            timer = 0f;
            OnNewDay();
        }

        IsNight = timer >= dayDuration;
    }

    void OnNewDay()
    {
        CurrentDay++;
        Debug.Log("New Day: " + CurrentDay);
    }

    public void ForceDay()
    {
        timer = 0f;
        IsNight = false;
    }

    public void ForceNight()
    {
        timer = dayDuration;
        IsNight = true;
    }

    public void SetDay(int day)
    {
        CurrentDay = day;
        timer = 0f;
        IsNight = false;
    }
}
