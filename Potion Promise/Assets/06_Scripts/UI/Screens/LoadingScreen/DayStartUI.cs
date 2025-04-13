using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DayStartUI : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private GameSettingSO gameSettingSO;

    [SerializeField] private CanvasGroup panel;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text dayAmountText;
    [SerializeField] private float startDelay = 1;

    private void Start()
    {
        dayAmountText.text = GameDataManager.Instance.CurrentDay.ToString();
        panel.alpha = 0;
        dayText.alpha = 0;
        dayAmountText.alpha = 0;
        ShowRoutine();
    }

    private void ShowRoutine()
    {
        AudioManager.Instance.PlayDayStartSound();
        panel.DOFade(1, gameSettingSO.FadeInAnimation).SetDelay(startDelay).OnComplete(() =>
        {
            dayText.DOFade(1, gameSettingSO.FadeInAnimation).SetDelay(startDelay).OnComplete(() =>
            {
                dayAmountText.DOFade(1, gameSettingSO.FadeInAnimation).SetDelay(startDelay);
            });
        });
    }
}
