using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    public static event Action<RecipeSlotUI> OnSlotSelected;

    [SerializeField] private Image slotCardImage;
    [SerializeField] private Image potionImage;
    [SerializeField] private TMP_Text potionName;
    [SerializeField] private List<Image> potionStarImages = new();
    [SerializeField] private HorizontalLayoutGroup starLayoutGroup;
    [SerializeField] private float layoutSpacingAmountForTwoStars = -70;

    private Button cardSlotButton;
    private Sprite unselectedSlotCardSprite;
    private Sprite selectedSlotCardSprite;

    private void Awake()
    {
        cardSlotButton = GetComponent<Button>();
    }

    private void Start()
    {
        OnSlotSelected += UnSelect;
    }

    public void Initialize(PotionData potionData, Sprite activePotionStar, Sprite unselectedSlotCardSprite, Sprite selectedSlotCardSprite, Action<PotionData> onButtonClicked)
    {
        cardSlotButton.onClick.AddListener(() =>
        {
            onButtonClicked(potionData);
            Select();
        });

        this.unselectedSlotCardSprite = unselectedSlotCardSprite;
        this.selectedSlotCardSprite = selectedSlotCardSprite;

        slotCardImage.sprite = unselectedSlotCardSprite;
        potionImage.sprite = potionData.Sprite;
        potionName.text = potionData.Name;

        // Reset and set stars based on rarity
        foreach (var starImage in potionStarImages)
        {
            starImage.gameObject.SetActive(false);
        }

        for (int i = 0; i <= (int)potionData.Rarity; i++)
        {
            potionStarImages[i].sprite = activePotionStar;
            potionStarImages[i].gameObject.SetActive(true);
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

    private void UnSelect(RecipeSlotUI recipeSlotUI)
    {
        if (recipeSlotUI == this) return;

        slotCardImage.sprite = unselectedSlotCardSprite;
    }
}
