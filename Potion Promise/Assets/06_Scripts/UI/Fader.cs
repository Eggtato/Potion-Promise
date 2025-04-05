using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private float fadeTime = 0.3f;

    private void Start()
    {
        InstantHide();
    }

    public void Show(Action onCompleted)
    {
        fader.DOFade(1, fadeTime).OnComplete(() =>
        {
            onCompleted?.Invoke();
        });
    }

    public void Hide(Action onCompleted)
    {
        fader.DOFade(0, fadeTime).OnComplete(() =>
        {
            onCompleted?.Invoke();
        });
    }

    public void Show()
    {
        fader.DOFade(1, fadeTime);
    }

    public void Hide()
    {
        fader.DOFade(0, fadeTime);
    }

    public void InstantHide()
    {
        fader.DOFade(0, 0);
    }
}
