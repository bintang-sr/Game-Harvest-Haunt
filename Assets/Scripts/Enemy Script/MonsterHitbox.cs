using UnityEngine;

public class MonsterHitbox : MonoBehaviour
{
    public int damage = 1;
    public AudioClip hitSound;
    public JumpscareUI jumpscareUI;

    AudioSource audioSource;
    bool hasHit;

    MonsterDetection detection;
    EnviromentBGM bgm;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        detection = FindObjectOfType<MonsterDetection>();
        bgm = FindObjectOfType<EnviromentBGM>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasHit) return;

        hasHit = true;

        detection?.StopAudio();
        bgm?.StopBGM();

        other.GetComponent<PlayerHealth>()?.TakeJumpscareDamage(damage);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ResetInventoryByJumpscare();
        }

        if (hitSound)
            audioSource.PlayOneShot(hitSound, 1.5f);

        float duration = hitSound ? hitSound.length : 1f;

        jumpscareUI?.Show(duration);

        Invoke(nameof(ResumeAudio), duration);
    }

    void ResumeAudio()
    {
        detection?.ResumeAudio();
        bgm?.ResumeBGM();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            hasHit = false;
    }
}
