using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class PauseUI : BaseUI
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitGame;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            Hide();
            Time.timeScale = 1;
        });
        settingButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            playerEventSO.Event.OnSettingButtonClicked?.Invoke();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();

            GameSceneManager.Instance.LoadMainMenuScene();
            DayProgressionManager.Instance.SetNullProgressionType();
            Hide();
            Time.timeScale = 1;
        });
        exitGame.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            Application.Quit();
        });
    }

    private void Start()
    {
        InstantHide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameSceneManager.Instance.CurrentScene != "MainMenu")
        {
            InstantShow();
            Time.timeScale = 0;
        }
    }
}
