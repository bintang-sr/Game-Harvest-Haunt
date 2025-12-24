using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayLightCycle : MonoBehaviour
{
    private Light2D sun;

    public float dayIntensity = 1.2f;
    public float nightIntensity = 0.1f;

    public Gradient dayGradient;
    public Gradient nightGradient; 

    private void Awake()
    {
        sun = GetComponent<Light2D>();
    }

    void Update()
    {
        DayCycle dc = DayCycle.Instance;
        float t = dc.DayProgress;

        float nightStart =
            (dc.dayDuration - dc.nightDuration) / dc.dayDuration;

        if (t < nightStart)
        {
            float dayT = Mathf.InverseLerp(0f, nightStart, t);

            sun.color = dayGradient.Evaluate(dayT);
            sun.intensity = dayIntensity;
        }
        else
        {
            float nightT = Mathf.InverseLerp(nightStart, 1f, t);

            sun.color = nightGradient.Evaluate(nightT);
            sun.intensity = Mathf.Lerp(dayIntensity, nightIntensity, nightT);
        }
    }
}
