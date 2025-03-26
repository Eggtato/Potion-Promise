using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerRoomUI : BaseUI
{
    [Header("NPC UI Reference")]
    [SerializeField] private CanvasGroup npcPanel;
    [SerializeField] private Image npcImage;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Button rejectButton;

    [Header("Animation")]
    [SerializeField] private float fadeInAnimation = 0.2f;

    public void InitializeNPC(Sprite sprite, ShopCustomerOrderData shopCustomerOrderData, Action rejectButtonAction)
    {
        npcImage.sprite = sprite;
        orderText.text = shopCustomerOrderData.OrderDescription;

        // Ensure the panel is visible
        npcPanel.alpha = 0;
        npcPanel.gameObject.SetActive(true);
        npcPanel.DOFade(1f, fadeInAnimation);

        // Remove old listeners before adding a new one
        rejectButton.onClick.RemoveAllListeners();
        rejectButton.onClick.AddListener(() =>
        {
            npcPanel.DOFade(0, 0.2f).OnComplete(() =>
            {
                npcPanel.gameObject.SetActive(false);
                rejectButtonAction?.Invoke(); // Generate a new order
            });
        });
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnCustomerRoomOpened += HandleCustomerRoomOpened;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnCustomerRoomOpened -= HandleCustomerRoomOpened;
    }

    private void HandleCustomerRoomOpened()
    {
        Show();
    }
}
