using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCustomerRoomUI : BaseUI
{
    [Header("NPC UI")]
    [SerializeField] private CanvasGroup npcPanel;
    [SerializeField] private Image npcImage;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Button rejectButton;

    [Header("Counter")]
    [SerializeField] private string openString = "OPEN";
    [SerializeField] private string closeString = "CLOSE";
    [SerializeField] private Button openShopButton;

    [Header("Animation")]
    [SerializeField] private float fadeInAnimation = 0.2f;
    [SerializeField] private float delayTime = 3f;

    private ShopCustomerOrderData currentCustomerOrderData;
    private List<InventoryPotionSlotUI> slotPool = new List<InventoryPotionSlotUI>();
    private ShopCustomerManager shopCustomerManager;

    private void Start()
    {
        npcPanel.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnCustomerRoomOpened += HandleCustomerRoomOpened;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnCustomerRoomOpened -= HandleCustomerRoomOpened;
        }
    }

    public void Initialize(ShopCustomerManager shopCustomerManager)
    {
        this.shopCustomerManager = shopCustomerManager;

        rejectButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            npcPanel.DOFade(0, 0.2f).OnComplete(() =>
            {
                npcPanel.gameObject.SetActive(false);
                shopCustomerManager.RejectOrder();
            });
        });

        openShopButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnOpenShopButtonClicked?.Invoke();
            openShopButton.GetComponentInChildren<TMP_Text>().text = closeString;
        });
    }


    public void InitializeNPC(Sprite sprite, ShopCustomerOrderData shopCustomerOrderData)
    {
        if (npcPanel.gameObject.activeSelf)
        {
            SetupNewNPC(sprite, shopCustomerOrderData);
            npcPanel.DOFade(0, 0.2f).OnComplete(() =>
            {
                npcPanel.gameObject.SetActive(false);
                SetupNewNPC(sprite, shopCustomerOrderData);
            });
        }
        else
        {
            SetupNewNPC(sprite, shopCustomerOrderData);
        }
    }

    private void SetupNewNPC(Sprite sprite, ShopCustomerOrderData shopCustomerOrderData)
    {
        currentCustomerOrderData = shopCustomerOrderData;

        npcImage.sprite = sprite;
        orderText.text = shopCustomerOrderData.OrderDescription;

        npcPanel.alpha = 0;
        npcPanel.gameObject.SetActive(true);
        npcPanel.DOFade(1f, fadeInAnimation);

        rejectButton.gameObject.SetActive(true);
    }

    public void HandleCorrectCustomerOrder()
    {
        orderText.text = currentCustomerOrderData.CorrectOrderDescription;
        rejectButton.gameObject.SetActive(false);
        npcPanel.DOFade(0, 0.2f).SetDelay(delayTime).OnComplete(() =>
        {
            npcPanel.gameObject.SetActive(false);
            shopCustomerManager.RejectOrder();
        });
    }

    public void HandleIncorrectCustomerOrder()
    {
        orderText.text = currentCustomerOrderData.InCorrectOrderDescription;
        rejectButton.gameObject.SetActive(false);
        npcPanel.DOFade(0, 0.2f).SetDelay(2f).OnComplete(() =>
        {
            npcPanel.gameObject.SetActive(false);
            shopCustomerManager.RejectOrder();
        });
    }


    private void HandleCustomerRoomOpened()
    {
        Show();
    }

}
