using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    [SerializeField] private Image slotCardImage;
    [SerializeField] private Image potionImage;
    [SerializeField] private TMP_Text potionName;
    [SerializeField] private List<Image> potionStarImages = new();
    [SerializeField] private HorizontalLayoutGroup starLayoutGroup;
    [SerializeField] private float layoutSpacingAmountForTwoStars = -70;

    public void Initialize(PotionData potionData, Sprite activePotionStar, Sprite slotCardSprite)
    {
        slotCardImage.sprite = slotCardSprite;
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
}
