using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftedPotionUI : BaseUI
{
    [Header("Project References")]
    [SerializeField] private GameAssetSO gameAssetSO;

    [Header("UI Elements")]
    [SerializeField] private Image potionImage;
    [SerializeField] private List<Image> potionStarImages = new();
    [SerializeField] private TMP_Text potionName;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(HandleClose);
    }

    private void Start()
    {
        InstantHide();
    }

    public void DisplayPotionSuccess(PotionData craftedPotion)
    {
        Show();

        // Set potion details
        potionImage.sprite = craftedPotion.Sprite;
        potionName.text = craftedPotion.Name;
        descriptionText.text = "Potion Obtained!";

        // Reset and set stars based on rarity
        foreach (var starImage in potionStarImages)
        {
            starImage.sprite = gameAssetSO.InActiveStar;
            starImage.gameObject.SetActive(false);
        }

        for (int i = 0; i <= (int)craftedPotion.Rarity; i++)
        {
            potionStarImages[i].sprite = gameAssetSO.ActiveStar;
            potionStarImages[i].gameObject.SetActive(true);
        }
    }

    public void DisplayPotionFailure()
    {
        Show();

        // Set failure feedback
        potionImage.sprite = gameAssetSO.FailedCraftedPotion;
        potionName.text = "Unknown Potion";
        descriptionText.text = "Crafting Failed!";

        // Reset stars
        foreach (var starImage in potionStarImages)
        {
            starImage.sprite = gameAssetSO.InActiveStar;
            starImage.gameObject.SetActive(false);
        }
    }

    private void HandleClose()
    {
        Hide();
    }
}
