using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    public static event Action<RecipeSlotUI> OnSlotSelected;

    [SerializeField] private Image slotCardImage;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private List<Image> itemStarImages = new();
    [SerializeField] private HorizontalLayoutGroup starLayoutGroup;
    [SerializeField] private float layoutSpacingAmountForTwoStars = -70;

    private Button cardSlotButton;
    private Sprite unselectedSlotCardSprite;
    private Sprite selectedSlotCardSprite;

    private PotionData potionData;
    private MaterialData materialData;

    private void Awake()
    {
        cardSlotButton = GetComponent<Button>();
    }

    private void Start()
    {
        OnSlotSelected += UnSelect;
    }

    public void Initialize(
    PotionData potionData,
    PlayerEventSO playerEventSO,
    Sprite activePotionStar,
    Sprite unselectedSlotCardSprite,
    Sprite selectedSlotCardSprite)
    {
        this.potionData = potionData;
        SetupSlot(potionData.Sprite, potionData.Name, (int)potionData.Rarity, activePotionStar, unselectedSlotCardSprite, selectedSlotCardSprite);
        cardSlotButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnPotionSlotClicked?.Invoke(potionData);
            Select();
        });
    }

    public void Initialize(
        MaterialData materialData,
        PlayerEventSO playerEventSO,
        Sprite activeMaterialStar,
        Sprite unselectedSlotCardSprite,
        Sprite selectedSlotCardSprite)
    {
        this.materialData = materialData;
        SetupSlot(materialData.Sprite, materialData.Name, (int)materialData.Rarity, activeMaterialStar, unselectedSlotCardSprite, selectedSlotCardSprite);
        cardSlotButton.onClick.AddListener(() =>
        {
            playerEventSO.Event.OnMaterialSlotClicked?.Invoke(materialData);
            Select();
        });
    }

    private void SetupSlot(
        Sprite icon,
        string name,
        int rarity,
        Sprite activeStarSprite,
        Sprite unselectedCardSprite,
        Sprite selectedCardSprite)
    {
        this.unselectedSlotCardSprite = unselectedCardSprite;
        this.selectedSlotCardSprite = selectedCardSprite;

        slotCardImage.sprite = unselectedCardSprite;
        itemIcon.sprite = icon;
        if (itemName) itemName.text = name;

        // Reset and set stars based on rarity
        foreach (var starImage in itemStarImages)
        {
            starImage.gameObject.SetActive(false);
        }

        for (int i = 0; i <= rarity && i < itemStarImages.Count; i++)
        {
            itemStarImages[i].sprite = activeStarSprite;
            itemStarImages[i].gameObject.SetActive(true);
        }

        if (rarity == 1)
        {
            starLayoutGroup.spacing = layoutSpacingAmountForTwoStars;
        }
    }


    private void Select()
    {
        slotCardImage.sprite = selectedSlotCardSprite;
        OnSlotSelected?.Invoke(this);
    }

    private void UnSelect(RecipeSlotUI recipeSlotUI)
    {
        if (recipeSlotUI == this) return;

        slotCardImage.sprite = unselectedSlotCardSprite;
    }
}
