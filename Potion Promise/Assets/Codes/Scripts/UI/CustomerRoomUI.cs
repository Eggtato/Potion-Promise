using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerRoomUI : BaseUI
{
    [Header("Project Reference")]
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;

    [Header("NPC UI Reference")]
    [SerializeField] private CanvasGroup npcPanel;
    [SerializeField] private Image npcImage;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Button rejectButton;

    [Header("NPC UI Reference")]
    [SerializeField] private InventoryPotionSlotUI inventorySlotTemplate;
    [SerializeField] private Transform inventoryParent;

    [Header("Animation")]
    [SerializeField] private float fadeInAnimation = 0.2f;
    [SerializeField] private float delayTime = 3f;

    private ShopCustomerOrderData currentCustomerOrderData;
    private List<InventoryPotionSlotUI> slotPool = new List<InventoryPotionSlotUI>();
    private Action onNextCustomer;
    private ShopCustomerManager shopCustomerManager;

    private void Start()
    {
        if (inventorySlotTemplate != null)
            inventorySlotTemplate.gameObject.SetActive(false);

        var craftedPotions = GameDataManager.Instance?.CraftedPotionDataList;

        if (craftedPotions == null || craftedPotions.Count == 0)
        {
            Debug.LogError("CraftedPotionDataList is NULL or EMPTY at Start()!");
            return;
        }

        npcPanel.gameObject.SetActive(false);

        GenerateInventory(craftedPotions);
    }

    public void Initialize(ShopCustomerManager shopCustomerManager)
    {
        this.shopCustomerManager = shopCustomerManager;

        rejectButton.onClick.AddListener(() =>
        {
            npcPanel.DOFade(0, 0.2f).OnComplete(() =>
            {
                npcPanel.gameObject.SetActive(false);
                shopCustomerManager.RejectOrder();
            });
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
            onNextCustomer = null;
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
            onNextCustomer = null;
        });
    }

    private void GenerateInventory(List<CraftedPotionData> craftedPotionDatas)
    {
        if (craftedPotionDatas == null || craftedPotionDatas.Count == 0)
        {
            Debug.LogError("GenerateInventory: CraftedPotionData list is null or empty!");
            return;
        }

        int index = 0;

        foreach (var craftedPotion in craftedPotionDatas)
        {
            if (index < slotPool.Count)
            {
                slotPool[index].gameObject.SetActive(true);
            }
            else
            {
                if (inventorySlotTemplate == null)
                {
                    Debug.LogError("InventorySlotTemplate is NULL!");
                    return;
                }

                var slotUI = Instantiate(inventorySlotTemplate, inventoryParent);
                slotUI.gameObject.SetActive(true);
                slotPool.Add(slotUI);
            }

            slotPool[index].Initialize(craftedPotion, potionDatabaseSO.GetPotionData(craftedPotion.PotionType));
            index++;
        }

        // Hide unused slots
        for (int i = index; i < slotPool.Count; i++)
        {
            slotPool[i].gameObject.SetActive(false);
        }
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

        var craftedPotions = GameDataManager.Instance?.CraftedPotionDataList;
        GenerateInventory(craftedPotions);
    }

}
