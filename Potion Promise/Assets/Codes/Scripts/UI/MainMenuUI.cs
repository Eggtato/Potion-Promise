using MoreMountains.Feedbacks;
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

    private void OnNewGameButtonClicked()
    {
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
    private void OnContinueButtonClicked()
    {
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
    private void OnSettingButtonClicked()
    {
        playerEventSO.Event.OnSettingButtonClicked?.Invoke();
    }
    private void OnExitButtonClicked()
    {
        playerEventSO.Event.OnExitButtonClicked?.Invoke();
    }
}
