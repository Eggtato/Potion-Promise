using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class SettingTabUI : MonoBehaviour
{
    [SerializeField] private Button tabButton;
    [SerializeField] private CanvasGroup page;
    [SerializeField] protected float fadeTransitionTime = 0.1f;
    [SerializeField] private MMFeedbacks buttonPressedFeedbacks;
    [SerializeField] private MMFeedbacks buttonUnpressedFeedbacks;

    private PlayerEventSO playerEventSO;

    private void Awake()
    {
        playerEventSO = GetComponent<SettingUI>().PlayerEventSO;

        tabButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnSettingTabButtonClicked?.Invoke(this);
            buttonPressedFeedbacks.PlayFeedbacks();
            AudioManager.Instance.PlayClickSound();
        });
    }

    private void Start()
    {

    }

    protected virtual void OnEnable()
    {
        playerEventSO.Event.OnSettingTabButtonClicked += OnSettingTabButtonClicked;
    }

    protected virtual void OnDisable()
    {
        playerEventSO.Event.OnSettingTabButtonClicked -= OnSettingTabButtonClicked;
    }

    private void OnSettingTabButtonClicked(SettingTabUI settingTabUI)
    {
        if (settingTabUI == this)
        {
            page.gameObject.SetActive(true);
            page.DOFade(1, fadeTransitionTime);
        }
        else
        {
            page.DOFade(0, fadeTransitionTime).OnComplete(() =>
            {
                page.gameObject.SetActive(false);
            });
        }
    }

    public void Show()
    {
        page.gameObject.SetActive(true);
        page.alpha = 0;
        page.DOFade(1, fadeTransitionTime);

        buttonPressedFeedbacks.PlayFeedbacks();
    }

    public void Hide()
    {
        buttonUnpressedFeedbacks.PlayFeedbacks();

        page.alpha = 1;
        page.DOFade(0, fadeTransitionTime).OnComplete(() =>
        {
            page.gameObject.SetActive(false);
        });
    }

    public void InstantHide()
    {
        page.alpha = 0;
        page.gameObject.SetActive(false);
    }
}
