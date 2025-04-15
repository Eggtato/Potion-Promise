using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopDayEndUI : BaseUI
{
    [Header("Project Reference")]
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;

    [SerializeField] private InventoryPotionSlotUI inventoryPotionSlotUI;
    [SerializeField] private Transform parent;
    [SerializeField] private TMP_Text todayRevenueAmountText;
    [SerializeField] private TMP_Text debtAmountText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private Button continueButton;

    [Header("Delay")]
    [SerializeField] private float delayAtStart = 2f;
    [SerializeField] private float delayInBetween = 0.01f;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            playerEventSO.Event.OnGoToNextScene();
        });
    }

    private void RefreshUI(int todayRevenueAmount, int debtAmount)
    {
        todayRevenueAmountText.text = "<sprite name=coin>" + todayRevenueAmount;
        debtAmountText.text = "<sprite name=coin>" + debtAmount;
    }

    public IEnumerator ShowResultUI(int todayRevenueAmount, int debtAmount, int currentDay, List<CraftedPotionData> craftedPotionDatas)
    {
        dayText.text = "DAY " + currentDay.ToString();
        RefreshUI(todayRevenueAmount, debtAmount);

        // Cleanup first
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in craftedPotionDatas)
        {
            PotionData potionData = potionDatabaseSO.PotionDataList.Find(i => i.PotionType == item.PotionType);

            var potion = Instantiate(inventoryPotionSlotUI, parent);
            potion.Initialize(item, potionData);
        }

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
