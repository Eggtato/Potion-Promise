using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ShopDayEndUI : BaseUI
{
    [SerializeField] private TMP_Text todayRevenueAmountText;
    [SerializeField] private TMP_Text debtAmountText;
    [SerializeField] private Button continueButton;

    [Header("Delay")]
    [SerializeField] private float delayAtStart = 2f;
    [SerializeField] private float delayInBetween = 0.01f;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnGoToNextScene();
            AudioManager.Instance.PlayClickSound();
        });
    }

    private void RefreshUI(int todayRevenueAmount, int debtAmount)
    {
        todayRevenueAmountText.text = "<sprite name=coin>" + todayRevenueAmount;
        debtAmountText.text = "<sprite name=coin>" + debtAmount;
    }

    public IEnumerator StartDebtConversion(int todayRevenueAmount, int debtAmount)
    {
        RefreshUI(todayRevenueAmount, debtAmount);
        yield return new WaitForSeconds(delayAtStart);

        while (todayRevenueAmount > 0)
        {
            todayRevenueAmount--;
            debtAmount--;

            AudioManager.Instance.PlayCoinSound(SoundLength.Short);
            RefreshUI(todayRevenueAmount, debtAmount);

            yield return new WaitForSeconds(delayInBetween);
        }
    }
}
