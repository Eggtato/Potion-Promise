using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialRecipeSlotUI : MonoBehaviour
{
    public static event Action<MaterialRecipeSlotUI> OnSlotSelected;

    [SerializeField] private Image slotCardImage;
    [SerializeField] private Image itemIcon;
    [SerializeField] private List<Image> itemStarImages = new();
    [SerializeField] private HorizontalLayoutGroup starLayoutGroup;
    [SerializeField] private float layoutSpacingAmountForTwoStars = -70;

    private Button cardSlotButton;
    private Sprite unselectedSlotCardSprite;
    private Sprite selectedSlotCardSprite;

    private void Awake()
    {
        cardSlotButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        OnSlotSelected += UnSelect;
    }

    private void OnDisable()
    {
        OnSlotSelected -= UnSelect;
    }

    public void Initialize(MaterialData materialData, PlayerEventSO playerEventSO, GameAssetSO gameAssetSO)
    {
        SetupSlot(materialData, gameAssetSO);
        cardSlotButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnMaterialSlotClicked?.Invoke(materialData);
            Select();
        });
    }

    private void SetupSlot(MaterialData materialData, GameAssetSO gameAssetSO)
    {
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
        itemIcon.sprite = materialData.Sprite;

        // Reset and set stars based on rarity
        foreach (var starImage in itemStarImages)
        {
            starImage.gameObject.SetActive(false);
        }

        for (int i = 0; i <= (int)materialData.Rarity && i < itemStarImages.Count; i++)
        {
            itemStarImages[i].sprite = gameAssetSO.ActiveStar;
            itemStarImages[i].gameObject.SetActive(true);
        }

        if ((int)materialData.Rarity == 1)
        {
            starLayoutGroup.spacing = layoutSpacingAmountForTwoStars;
        }
    }


    private void Select()
    {
        slotCardImage.sprite = selectedSlotCardSprite;
        OnSlotSelected?.Invoke(this);
    }

    private void UnSelect(MaterialRecipeSlotUI recipeSlotUI)
    {
        if (recipeSlotUI == this) return;

        slotCardImage.sprite = unselectedSlotCardSprite;
    }
}