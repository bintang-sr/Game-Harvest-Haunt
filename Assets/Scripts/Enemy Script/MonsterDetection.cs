using UnityEngine;
using System.Collections;

public class MonsterDetection : MonoBehaviour
{
    public MonsterAI ai;
    public Transform player;
    public AudioClip detectSound;

    public float maxVolume = 1.5f;
    public float minDistance = 1.5f;
    public float maxDistance = 6f;
    public float fadeSpeed = 2f;

    AudioSource audioSource;
    bool playerInside;
    Coroutine fadeRoutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;

        if (detectSound)
            audioSource.clip = detectSound;
    }

    void Update()
    {
        if (!playerInside || !player) return;

        float dist = Vector2.Distance(transform.position, player.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, dist);
        float targetVolume = Mathf.Lerp(0f, maxVolume, t);

        audioSource.volume = Mathf.MoveTowards(
            audioSource.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        ai.PlayerDetected = true;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        ai.PlayerDetected = false;

        if (gameObject.activeInHierarchy)
            fadeRoutine = StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                0f,
                fadeSpeed * Time.deltaTime
            );
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0f;
    }

    public void StopAudio()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        audioSource.Stop();
        audioSource.volume = 0f;
        playerInside = false;
    }

    public void ResumeAudio()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
