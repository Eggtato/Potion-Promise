using UnityEngine;
using UnityEngine.UI;

public class NewGameConfirmationUI : BaseUI
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        yesButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            GameDataManager.Instance.ClearAllData();
            playerEventSO.Event.OnGoToNextScene?.Invoke();
        });

        noButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            Hide();
        });
    }

    private void Start()
    {
        InstantHide();
    }
}
