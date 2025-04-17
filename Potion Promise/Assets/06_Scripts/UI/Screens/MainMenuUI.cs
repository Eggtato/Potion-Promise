using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [Header("Buttons")]
    [SerializeField] private NewGameConfirmationUI newGameConfirmationUI;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        loadGameButton.onClick.AddListener(OnContinueButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Start()
    {
        MMSoundManager.Instance.UnmuteMusic();

        if (GameDataManager.Instance.ProgressionDataList.IsNullOrEmpty())
        {
            loadGameButton.gameObject.SetActive(false);
        }
    }

    private void OnNewGameButtonClicked()
    {
        AudioManager.Instance.PlayClickSound();

        if (GameDataManager.Instance.ProgressionDataList.IsNullOrEmpty())
        {
            AudioManager.Instance.PlayClickSound();
            playerEventSO.Event.OnGoToNextScene?.Invoke();
        }
        else
        {
            newGameConfirmationUI.Show();
        }
    }
    private void OnContinueButtonClicked()
    {
        AudioManager.Instance.PlayClickSound();
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
    private void OnSettingButtonClicked()
    {
        AudioManager.Instance.PlayClickSound();
        playerEventSO.Event.OnSettingButtonClicked?.Invoke();
    }
    private void OnExitButtonClicked()
    {
        AudioManager.Instance.PlayClickSound();
        playerEventSO.Event.OnExitButtonClicked?.Invoke();
        Application.Quit();
    }
}
