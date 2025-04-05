using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionRecipeSlotUI : MonoBehaviour
{
    public static event Action<PotionRecipeSlotUI> OnSlotSelected;

    [SerializeField] private Image slotCardImage;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
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

    public void Initialize(PotionData potionData, PlayerEventSO playerEventSO, GameAssetSO gameAssetSO)
    {
        SetupSlot(potionData, gameAssetSO);
        cardSlotButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnPotionSlotClicked?.Invoke(potionData);
            Select();
        });
    }

    private void SetupSlot(PotionData potionData, GameAssetSO gameAssetSO)
    {
        switch ((int)potionData.Rarity)
        {
            case 0:
                this.unselectedSlotCardSprite = gameAssetSO.PotionCommmonCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedPotionCommmonCard;
                break;
            case 1:
                this.unselectedSlotCardSprite = gameAssetSO.PotionRareCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedPotionRareCard;
                break;
            case 2:
                this.unselectedSlotCardSprite = gameAssetSO.PotionEpicCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedPotionEpicCard;
                break;

        }


        slotCardImage.sprite = unselectedSlotCardSprite;
        itemIcon.sprite = potionData.Sprite;

        if (itemName) itemName.text = potionData.Name;

        // Reset and set stars based on rarity
        foreach (var starImage in itemStarImages)
        {
            starImage.gameObject.SetActive(false);
        }

        for (int i = 0; i <= (int)potionData.Rarity && i < itemStarImages.Count; i++)
        {
            itemStarImages[i].sprite = gameAssetSO.ActiveStar;
            itemStarImages[i].gameObject.SetActive(true);
        }

        if ((int)potionData.Rarity == 1)
        {
            starLayoutGroup.spacing = layoutSpacingAmountForTwoStars;
        }
    }


    private void Select()
    {
        slotCardImage.sprite = selectedSlotCardSprite;
        OnSlotSelected?.Invoke(this);
    }

    private void UnSelect(PotionRecipeSlotUI recipeSlotUI)
    {
        if (recipeSlotUI == this) return;

        slotCardImage.sprite = unselectedSlotCardSprite;
    }
}
