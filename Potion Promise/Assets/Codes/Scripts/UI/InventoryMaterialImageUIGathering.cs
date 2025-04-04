using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUIGathering : MonoBehaviour, IPointerClickHandler
{
    public MaterialData MaterialData { get; private set; }

    private Transform rootcanvasParent;

    [SerializeField] private Image icon;
    private Transform parentAfterDrag;

    [SerializeField] private Image slotCardImage;

    private Sprite selectedSlotCardSprite;
    private Sprite unselectedSlotCardSprite;

    public bool canBeSelected = false;
    public bool selected = false;

    private RewardManagerUI rewardManager;


    private void Awake()
    {
        rootcanvasParent = GetComponentInParent<Canvas>().transform;
    }

    public void Initialize(MaterialData materialData, RewardManagerUI rewardManager, GameAssetSO gameAssetSO)
    {
        this.MaterialData = materialData;
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

            rewardManager.SetMaterialSelected(MaterialData);

            selected = true;
        }
    }

    public void Unselect()
    {
        selected = false;

        slotCardImage.sprite = unselectedSlotCardSprite;
    }
}
