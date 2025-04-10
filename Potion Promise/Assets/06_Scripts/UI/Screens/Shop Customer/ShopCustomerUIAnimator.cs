using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using Febucci.UI;
using System;

public class ShopCustomerUIAnimator : MonoBehaviour
{
    [SerializeField] private CanvasGroup orderPanel;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private CanvasGroup rejectButtonCanvasGroup;

    public void AnimateCustomerEntry(ShopCustomerImageUI customerUI, ShopCustomerOrderData orderData)
    {
        orderPanel.DOFade(1, 0.5f);
        orderText.text = orderData.CorrectOrderDescription;

        var typewriter = orderText.GetComponent<TypewriterByCharacter>();
        typewriter.StartShowingText();

        rejectButtonCanvasGroup.DOFade(1, 0.5f);
        rejectButtonCanvasGroup.blocksRaycasts = true;
    }

    public IEnumerator ShowCustomerOrderResult(string resultText, Action onComplete)
    {
        orderText.text = resultText;
        yield return null;

        var typewriter = orderText.GetComponent<TypewriterByCharacter>();
        typewriter.StartShowingText();

        rejectButtonCanvasGroup.DOFade(0, 0.5f);
        rejectButtonCanvasGroup.blocksRaycasts = false;

        typewriter.onTextShowed.RemoveAllListeners();
        typewriter.onTextShowed.AddListener(() =>
        {
            orderPanel.DOFade(0, 0.5f);
            onComplete?.Invoke();
        });
    }
}

