using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryMaterialSlotUIGathering : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text quantityText;
    public ObtainedMaterialData obtainedMaterialData { get; private set; }
    public MaterialData materialData { get; private set; }

    [SerializeField] private Image icon;
    private Transform parentAfterDrag;

    [SerializeField] private Image slotCardImage;

    private Sprite selectedSlotCardSprite;
    private Sprite unselectedSlotCardSprite;

    public bool canBeSelected = false;
    public bool selected = false;

    private RewardManagerUI rewardManager;


    public void Initialize(ObtainedMaterialData obtainedMaterialData, MaterialData materialData, RewardManagerUI rewardManager, GameAssetSO gameAssetSO)
    {
        this.obtainedMaterialData = obtainedMaterialData;
        this.materialData = materialData;
        quantityText.text = "x" + obtainedMaterialData.Quantity;

        this.materialData = materialData;
        icon.sprite = materialData.Sprite;
        this.rewardManager = rewardManager;

        switch ((int)materialData.Rarity)
        {
            case 0:
                this.unselectedSlotCardSprite = gameAssetSO.MaterialCommmonCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedMaterialCommmonCard;
                break;
            case 1:
                this.unselectedSlotCardSprite = gameAssetSO.MaterialRareCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedMaterialRareCard;
                break;
            case 2:
                this.unselectedSlotCardSprite = gameAssetSO.MaterialEpicCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedMaterialEpicCard;
                break;

        }

        slotCardImage.sprite = unselectedSlotCardSprite;
    }

    public void AddQuantity()
    {
        obtainedMaterialData.Quantity += 1;
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!canBeSelected) return;

        if (selected)
        {
            rewardManager.UnselectAll();
        }
        else
        {

            slotCardImage.sprite = selectedSlotCardSprite;

            rewardManager.SetMaterialSelected(materialData);

            selected = true;
        }
    }

    public void Unselect()
    {
        selected = false;

        slotCardImage.sprite = unselectedSlotCardSprite;
    }
}
