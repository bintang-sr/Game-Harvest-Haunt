using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    public GameObject gameOverPanel;

    public AudioSource audioSource;
    public AudioClip gameOverSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        float waitTime = 2f;

        if (audioSource && gameOverSound)
        {
            audioSource.PlayOneShot(gameOverSound);
            waitTime = gameOverSound.length;
        }

        StartCoroutine(ReturnToMainMenu(waitTime));
    }

    IEnumerator ReturnToMainMenu(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
