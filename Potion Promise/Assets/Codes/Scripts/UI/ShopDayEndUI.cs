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

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnDayEnd += HandleDayEnd;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnDayEnd -= HandleDayEnd;
        }
    }

    private void HandleDayEnd()
    {
        AudioManager.Instance.PlayDayEndSound();
        Show();
        StartCoroutine(ProcessConvertion(GameLevelManager.Instance.EarnedCoin, GameDataManager.Instance.Debt));
    }

    private void RefreshUI(int todayRevenueAmount, int debtAmount)
    {
        todayRevenueAmountText.text = "<sprite name=coin>" + todayRevenueAmount;
        debtAmountText.text = "<sprite name=coin>" + debtAmount;
    }

    private IEnumerator ProcessConvertion(int todayRevenueAmount, int debtAmount)
    {
        RefreshUI(todayRevenueAmount, debtAmount);
        yield return new WaitForSeconds(delayAtStart);

        while (todayRevenueAmount >= 0)
        {
            AudioManager.Instance.PlayCoinSound();
            RefreshUI(todayRevenueAmount, debtAmount);
            todayRevenueAmount--;
            debtAmount--;
            yield return new WaitForSeconds(delayInBetween);
        }
    }
}
