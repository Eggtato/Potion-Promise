using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        continueGameButton.onClick.AddListener(OnContinueButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Start()
    {
        MMSoundManager.Instance.UnmuteMusic();
    }

    private void OnNewGameButtonClicked()
    {
        AudioManager.Instance.PlayClickSound();
        playerEventSO.Event.OnGoToNextScene?.Invoke();
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
