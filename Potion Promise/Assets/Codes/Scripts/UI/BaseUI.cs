using UnityEngine;
using DG.Tweening;

public abstract class BaseUI : MonoBehaviour
{
    [SerializeField] protected PlayerEventSO playerEventSO;
    [SerializeField] protected CanvasGroup panel;
    [SerializeField] protected float fadeTransitionTime = 0.3f;

    protected virtual void OnEnable()
    {
        playerEventSO.Event.OnAnyUIClosed += HandleAnyUIClosed;
    }

    protected virtual void OnDisable()
    {
        playerEventSO.Event.OnAnyUIClosed -= HandleAnyUIClosed;
    }

    private void HandleAnyUIClosed()
    {
        InstantHide();
    }

    protected void Show()
    {
        // AudioManager.Instance.PlayClickSound();

        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, fadeTransitionTime);
    }

    protected void Hide()
    {
        // AudioManager.Instance.PlayClickSound();

        panel.alpha = 1;
        panel.DOFade(0, fadeTransitionTime).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }

    protected void InstantHide()
    {
        panel.alpha = 0;
        panel.gameObject.SetActive(false);
    }
}
