using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneUI : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private Button skipButton;
    [SerializeField] private CanvasGroup skipPanelCanvasGroup;
    [SerializeField] private CanvasGroup skipButtonCanvasGroup;

    [Header("Animation")]
    [SerializeField] private float skipButtonAppearDuration = 0.5f;
    [SerializeField] private float maxDissapearTime = 3f;

    private bool hasSkipButtonAppeared;
    private float dissaperTimer;

    private void Awake()
    {
        skipButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            playerEventSO.Event.OnGoToNextScene?.Invoke();
        });
    }

    private void Start()
    {
        skipPanelCanvasGroup.alpha = 0;
        skipButtonCanvasGroup.alpha = 0;
        skipPanelCanvasGroup.gameObject.SetActive(false);
        skipButtonCanvasGroup.gameObject.SetActive(false);
        dissaperTimer = maxDissapearTime;
    }

    void Update()
    {
        if (hasSkipButtonAppeared)
        {
            dissaperTimer -= Time.deltaTime;
            if (dissaperTimer <= 0)
            {
                skipPanelCanvasGroup.DOFade(0, skipButtonAppearDuration).OnComplete(() =>
                {
                    skipButtonCanvasGroup.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(false);
                });
                dissaperTimer = maxDissapearTime;
                hasSkipButtonAppeared = false;
            }
        }

        if (Input.anyKeyDown && !hasSkipButtonAppeared)
        {
            skipPanelCanvasGroup.gameObject.SetActive(true);
            skipPanelCanvasGroup.DOFade(1, skipButtonAppearDuration).OnComplete(() =>
            {
                skipButtonCanvasGroup.gameObject.SetActive(true);
                skipButtonCanvasGroup.DOFade(1, skipButtonAppearDuration);
            });
            hasSkipButtonAppeared = true;
        }
    }
}
