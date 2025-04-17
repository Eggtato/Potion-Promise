using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using Sirenix.Utilities;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class ShopDayEndUI : BaseUI
{
    [Header("Project Reference")]
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;
    [SerializeField] private InventoryPotionSlotUI inventoryPotionSlotUIPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private TMP_Text todayRevenueAmountText;
    [SerializeField] private TMP_Text debtAmountText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private Button continueButton;
    [SerializeField] private CanvasGroup continueButtonCanvasGroup;
    [SerializeField] private Image stampImage;
    [SerializeField] private RectTransform[] potionImages;

    [Header("Delay")]
    [SerializeField] private float delayAtStart = 2f;
    [SerializeField] private float delayInBetween = 0.01f;

    [Header("Animation")]
    [SerializeField] private Ease potionEase = Ease.OutBack;

    private readonly List<Vector3> startingLocalPositions = new();
    private readonly List<Vector3> startingLocalRotations = new();

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            playerEventSO.Event.OnGoToNextScene();
        });
    }

    private void Start()
    {
        // Store original transform states and reset
        foreach (var image in potionImages)
        {
            startingLocalPositions.Add(image.localPosition);
            startingLocalRotations.Add(image.localEulerAngles);

            image.localPosition = Vector3.zero;
            image.localRotation = Quaternion.identity;
        }

        // Setup UI state
        continueButtonCanvasGroup.alpha = 0f;
        continueButtonCanvasGroup.blocksRaycasts = false;
        stampImage.transform.localScale = Vector3.zero;
    }

    private void RefreshUI(int todayRevenueAmount, int debtAmount)
    {
        todayRevenueAmountText.text = $"<sprite name=coin>{todayRevenueAmount}";
        debtAmountText.text = $"<sprite name=coin>{debtAmount}";
    }

    public IEnumerator ShowResultUI(int todayRevenueAmount, int debtAmount, int currentDay, List<CraftedPotionData> craftedPotionDatas)
    {
        MMSoundManager.Instance.MuteMusic();

        dayText.text = $"DAY {currentDay}";
        RefreshUI(todayRevenueAmount, debtAmount);

        // Cleanup old potion slots
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        // Animate background potions into place
        for (int i = 0; i < potionImages.Length; i++)
        {
            potionImages[i]
                .DOLocalMove(startingLocalPositions[i], 0.5f)
                .SetEase(potionEase)
                .SetDelay(0.5f);

            potionImages[i]
                .DOLocalRotate(startingLocalRotations[i], 0.5f)
                .SetEase(potionEase)
                .SetDelay(0.5f);
        }

        AudioManager.Instance.PlayCoinSound(SoundLength.Long);
        yield return new WaitForSeconds(delayAtStart);

        // Show crafted potions
        foreach (var item in craftedPotionDatas)
        {
            var potionData = potionDatabaseSO.PotionDataList.Find(p => p.PotionType == item.PotionType);
            var potionSlot = Instantiate(inventoryPotionSlotUIPrefab, parent);
            potionSlot.Initialize(item, potionData, false);

            var potionCanvasGroup = potionSlot.GetComponent<CanvasGroup>();
            if (potionCanvasGroup != null)
            {
                potionCanvasGroup.alpha = 0f;
                potionCanvasGroup.DOFade(1f, 0.2f);
            }

            AudioManager.Instance.PlayPopSound();
            yield return new WaitForSeconds(delayInBetween * 20);
        }

        yield return new WaitForSeconds(0.5f);

        // Animate revenue/debt countdown
        while (todayRevenueAmount > 0)
        {
            todayRevenueAmount--;
            debtAmount--;
            AudioManager.Instance.PlayCoinSound(SoundLength.Short);
            RefreshUI(todayRevenueAmount, debtAmount);
            yield return new WaitForSeconds(delayInBetween);
        }

        yield return new WaitForSeconds(0.5f);

        // Stamp animation
        AudioManager.Instance.PlayStampSound();
        stampImage.transform.DOScale(1f, 0.2f).SetEase(Ease.InSine);

        yield return new WaitForSeconds(0.5f);

        // Show continue button
        continueButtonCanvasGroup.DOFade(1f, 0.2f).SetEase(Ease.InSine);
        continueButtonCanvasGroup.blocksRaycasts = true;
    }
}
