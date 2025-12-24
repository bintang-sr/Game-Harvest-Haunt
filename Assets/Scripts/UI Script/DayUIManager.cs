using UnityEngine;
using TMPro;
using System.Collections;

public class DayUIManager : MonoBehaviour
{
    public static DayUIManager Instance;

    public CanvasGroup dayCanvasGroup;
    public TMP_Text dayText;
    public AudioSource audioSource;
    public AudioClip daySound;

    public float displayDuration = 1.5f;
    public float fadeOutDuration = 0.5f;

    bool isTransitioning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        dayCanvasGroup.alpha = 0f;
    }

    void Start()
    {
        StartCoroutine(CheckDayCycle());
    }

    IEnumerator CheckDayCycle()
    {
        bool lastIsNight = DayCycle.Instance.IsNight;

        while (true)
        {
            yield return null;

            if (lastIsNight && !DayCycle.Instance.IsNight)
            {
                ShowDayUI();
            }

            lastIsNight = DayCycle.Instance.IsNight;
        }
    }

    public void ShowDayUI()
    {
        if (!isTransitioning)
            StartCoroutine(DoDayTransition());
    }

    IEnumerator DoDayTransition()
    {
        isTransitioning = true;

        dayText.text = "Day " + DayCycle.Instance.CurrentDay;

        if (audioSource && daySound)
            audioSource.PlayOneShot(daySound);

        dayCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            dayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }

        dayCanvasGroup.alpha = 0f;
        isTransitioning = false;
    }
}
