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

    private void Start()
    {
        // Ensure the inventory slot template is inactive by default
        if (inventorySlotTemplate != null)
            inventorySlotTemplate.gameObject.SetActive(false);

        var craftedPotions = GameDataManager.Instance?.CraftedPotionDataList;

        GenerateInventory(craftedPotions);
    }

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

    private void GenerateInventory(List<CraftedPotionData> craftedPotionDatas)
    {
        // Clear existing slots except the template
        foreach (Transform child in inventoryParent)
        {
            if (child.gameObject == inventorySlotTemplate.gameObject) continue;
            Destroy(child.gameObject);
        }

        // Create a slot for each obtained material
        foreach (var craftedPotion in craftedPotionDatas)
        {
            var potionData = potionDatabaseSO.PotionDataList
                .FirstOrDefault(p => p.PotionType == craftedPotion.PotionType);

            if (potionData == null)
            {
                Debug.LogWarning($"PotionData not found for type: {craftedPotion.PotionType}");
                continue;
            }

            var slotUI = Instantiate(inventorySlotTemplate, inventoryParent);
            slotUI.gameObject.SetActive(true);
            slotUI.Initialize(craftedPotion, potionData);
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
    }
}
