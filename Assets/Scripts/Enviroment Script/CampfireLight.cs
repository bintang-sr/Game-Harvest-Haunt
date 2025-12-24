using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampfireLight : MonoBehaviour
{
    private Light2D fireLight;

    public float nightIntensity = 2.5f;
    public float flickerSpeed = 8f;
    public float flickerAmount = 0.25f;

    public Color fireColor = new Color(1f, 0.6f, 0.3f);

    private float baseIntensity;

    void Awake()
    {
        fireLight = GetComponent<Light2D>();
        fireLight.color = fireColor;
    }

    void Update()
    {
        DayCycle dc = DayCycle.Instance;
        float t = dc.DayProgress;

        float nightStart =
            (dc.dayDuration - dc.nightDuration) / dc.dayDuration;

        if (t < nightStart)
        {
            fireLight.enabled = false;
        }
        else
        {
            fireLight.enabled = true;

            float nightT = Mathf.InverseLerp(nightStart, 1f, t);
            baseIntensity = Mathf.Lerp(0f, nightIntensity, nightT);

            float flicker =
                Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) * flickerAmount;

            fireLight.intensity = baseIntensity + flicker;
        }
    }
}
