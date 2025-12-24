using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject savePanel;
    public GameObject guidePanel;

    public Button[] saveSlotButtons;
    public Button closeSavePanelButton;
    public Button closePausePanelButton;
    public Button exitButton;
    public Button openSavePanelButton;

    public Button openGuidePanelButton; 
    public Button closeGuidePanelButton; 

    public GameSaveSystem saveSystem;

    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    void Start()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (savePanel) savePanel.SetActive(false);
        if (guidePanel) guidePanel.SetActive(false);

        // Buttons
        AddButtonListeners(closeSavePanelButton, CloseSavePanel);
        AddButtonListeners(closePausePanelButton, ClosePausePanel);
        AddButtonListeners(exitButton, ReturnToMainMenu);
        AddButtonListeners(openSavePanelButton, OpenSavePanel);
        AddButtonListeners(openGuidePanelButton, OpenGuidePanel);
        AddButtonListeners(closeGuidePanelButton, CloseGuidePanel);

        SetupSaveButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    void TogglePause()
    {
        if (!pausePanel) return;

        bool isActive = pausePanel.activeSelf;
        pausePanel.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }

    void ClosePausePanel()
    {
        if (!pausePanel) return;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void CloseSavePanel()
    {
        if (savePanel)
            savePanel.SetActive(false);
    }

    void OpenGuidePanel()
    {
        if (guidePanel)
        {
            guidePanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    void CloseGuidePanel()
    {
        if (guidePanel)
        {
            guidePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSavePanel()
    {
        if (!savePanel) return;

        UpdateSaveButtons();
        savePanel.SetActive(true);
    }

    void SetupSaveButtons()
    {
        if (saveSlotButtons == null) return;

        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            if (!saveSlotButtons[i]) continue;

            int slotNumber = i + 1;

            saveSlotButtons[i].onClick.RemoveAllListeners();
            saveSlotButtons[i].onClick.AddListener(() => SaveToSlot(slotNumber));

            AddButtonAudio(saveSlotButtons[i]);
        }
    }

    void UpdateSaveButtons()
    {
        if (saveSlotButtons == null) return;

        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            if (!saveSlotButtons[i]) continue;

            int slotNumber = i + 1;
            TMP_Text btnText = saveSlotButtons[i].GetComponentInChildren<TMP_Text>();

            if (!btnText) continue;

            if (saveSystem != null && saveSystem.HasSave(slotNumber))
            {
                GameSaveData data = saveSystem.GetSaveData(slotNumber);
                btnText.text = data != null
                    ? $"Day {data.currentDay}     {FormatTime(data.playTime)}"
                    : $"Slot {slotNumber} (Empty)";
            }
            else
            {
                btnText.text = $"Slot {slotNumber} (Empty)";
            }
        }
    }

    void SaveToSlot(int slot)
    {
        if (saveSystem == null) return;

        saveSystem.SaveGame(slot);
        UpdateSaveButtons();
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

