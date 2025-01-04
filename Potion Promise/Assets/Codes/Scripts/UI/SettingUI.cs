using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    [SerializeField] private Button backButton;

    [Header("Tabs")]
    [SerializeField] private SettingTabUI[] settingTabUIs;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void Start()
    {
        SetFirstPageToDisplay();
        InstantHide();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnSettingButtonClicked += OnSettingButtonClicked;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnSettingButtonClicked -= OnSettingButtonClicked;
    }

    private void SetFirstPageToDisplay()
    {
        foreach (var item in settingTabUIs)
        {
            item.InstantHide();
        }

        settingTabUIs[0].Select();
    }

    private void OnSettingButtonClicked()
    {
        Show();
    }

    private void OnBackButtonClicked()
    {
        Hide();
    }
}
