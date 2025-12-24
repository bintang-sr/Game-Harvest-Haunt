using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EnviromentBGM : MonoBehaviour
{
    public AudioClip dayBGM;
    public AudioClip nightBGM;

    public float targetVolume = 0.6f;
    public float fadeSpeed = 1.5f;

    AudioSource audioSource;
    Coroutine fadeRoutine;

    bool isNightPlaying;
    bool mutedByJumpscare;
    bool isFading;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }

    void Start()
    {
        ForceSyncDayNight();
        FadeIn();
    }

    void Update()
    {
        if (mutedByJumpscare || isFading) return;

        if (DayCycle.Instance.IsNight != isNightPlaying)
            StartDayNightSwitch(DayCycle.Instance.IsNight);
    }

    void StartDayNightSwitch(bool toNight)
    {
        StopAllCoroutines();
        fadeRoutine = StartCoroutine(DayNightRoutine(toNight));
    }

    IEnumerator DayNightRoutine(bool toNight)
    {
        isFading = true;

        yield return FadeTo(0f);

        audioSource.clip = toNight ? nightBGM : dayBGM;
        isNightPlaying = toNight;
        audioSource.Play();

        yield return FadeTo(targetVolume);

        isFading = false;
    }

    IEnumerator FadeTo(float volume)
    {
        while (!Mathf.Approximately(audioSource.volume, volume))
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                volume,
                fadeSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    void ForceSyncDayNight()
    {
        isNightPlaying = DayCycle.Instance.IsNight;
        audioSource.clip = isNightPlaying ? nightBGM : dayBGM;
        audioSource.volume = 0f;
        audioSource.Play();
    }

    public void FadeOut()
    {
        if (mutedByJumpscare) return;

        StopAllCoroutines();
        fadeRoutine = StartCoroutine(FadeTo(0f));
    }

    public void FadeIn()
    {
        if (mutedByJumpscare) return;

        StopAllCoroutines();

        if (!audioSource.isPlaying)
            audioSource.Play();

        fadeRoutine = StartCoroutine(FadeTo(targetVolume));
    }

    public void StopBGM()
    {
        mutedByJumpscare = true;
        isFading = false;

        StopAllCoroutines();
        fadeRoutine = null;

        audioSource.Stop();
        audioSource.volume = 0f;
    }

    public void ResumeBGM()
    {
        mutedByJumpscare = false;
        isFading = false;

        StopAllCoroutines();
        fadeRoutine = null;

        ForceSyncDayNight();

        audioSource.volume = 0f;
        audioSource.Play();

        fadeRoutine = StartCoroutine(FadeTo(targetVolume));
    }
}
