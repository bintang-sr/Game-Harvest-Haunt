using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainButtonsGroup; 

    public Button startButton;
    public Button loadButton;
    public Button exitButton;

    public GameObject loadPanel;
    public Button[] loadSlotButtons;
    public Button closeLoadPanelButton;

    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public AudioSource bgmSource;
    public AudioClip bgmClip;

    void Start()
    {
        if (bgmSource != null && bgmClip != null && !bgmSource.isPlaying)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        AddButtonListeners(startButton, StartGame);
        AddButtonListeners(loadButton, OpenLoadPanel);
        AddButtonListeners(exitButton, ExitGame);
        AddButtonListeners(closeLoadPanelButton, CloseLoadPanel);

        if (loadPanel != null)
            loadPanel.SetActive(false);

        UpdateLoadButtons();
    }

    void StartGame()
    {
        LoadContext.SelectedSlot = -1;
        SceneManager.LoadScene("Forest");
    }

    void OpenLoadPanel()
    {
        UpdateLoadButtons();

        if (mainButtonsGroup != null)
            mainButtonsGroup.SetActive(false);

        if (loadPanel != null)
            loadPanel.SetActive(true);
    }

    void CloseLoadPanel()
    {
        if (loadPanel != null)
            loadPanel.SetActive(false);

        if (mainButtonsGroup != null)
            mainButtonsGroup.SetActive(true);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void UpdateLoadButtons()
    {
        for (int i = 0; i < loadSlotButtons.Length; i++)
        {
            int slot = i + 1;
            Button btn = loadSlotButtons[i];
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

            bool hasSave = PlayerPrefs.HasKey($"saveData_slot{slot}");

            if (txt != null)
            {
                if (hasSave)
                {
                    string json = PlayerPrefs.GetString($"saveData_slot{slot}");
                    GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
                    txt.text = data != null ? $"Day {data.currentDay}     {FormatTime(data.playTime)}" : $"Slot {slot}";
                }
                else txt.text = $"Slot {slot} (Empty)";
            }

            btn.onClick.RemoveAllListeners();
            btn.interactable = hasSave;
            if (hasSave)
                btn.onClick.AddListener(() => LoadSlot(slot));

            AddButtonAudio(btn);
        }
    }

    void LoadSlot(int slot)
    {
        LoadContext.SelectedSlot = slot;
        SceneManager.LoadScene("Forest");
    }

    string FormatTime(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        return $"{m:D2}:{s:D2}";
    }

    void AddButtonListeners(Button btn, UnityEngine.Events.UnityAction action)
    {
        if (!btn) return;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(action);

        AddButtonAudio(btn);
    }

    void AddButtonAudio(Button btn)
    {
        if (!btn || audioSource == null) return;

        btn.onClick.AddListener(() =>
        {
            if (clickSound != null)
                audioSource.PlayOneShot(clickSound);
        });

        EventTriggerListener hover = btn.gameObject.GetComponent<EventTriggerListener>();
        if (hover == null)
        {
            hover = btn.gameObject.AddComponent<EventTriggerListener>();
        }
        hover.onEnter = () =>
        {
            if (hoverSound != null)
                audioSource.PlayOneShot(hoverSound);
        };
    }
}
