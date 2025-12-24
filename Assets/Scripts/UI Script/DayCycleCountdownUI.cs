using UnityEngine;
using TMPro;

public class DayCycleCountdownUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Update()
    {
        if (DayCycle.Instance == null) return;

        float remainingTime;

        if (DayCycle.Instance.IsDay)
            remainingTime = DayCycle.Instance.dayDuration - (DayCycle.Instance.DayProgress * (DayCycle.Instance.dayDuration + DayCycle.Instance.nightDuration));
        else
            remainingTime = (DayCycle.Instance.dayDuration + DayCycle.Instance.nightDuration) - (DayCycle.Instance.DayProgress * (DayCycle.Instance.dayDuration + DayCycle.Instance.nightDuration));

        remainingTime = Mathf.Max(0f, remainingTime);

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timeText.text = $"{minutes:00} : {seconds:00}";
    }
}
