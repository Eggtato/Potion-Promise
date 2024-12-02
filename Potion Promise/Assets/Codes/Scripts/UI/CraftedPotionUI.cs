using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftedPotionUI : BaseUI
{
    [Header("Project References")]
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;
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

    protected override void OnEnable()
    {
        playerEventSO.Event.OnCraftPotionButtonClicked += HandleCraftPotionButtonClicked;
    }

    protected override void OnDisable()
    {
        playerEventSO.Event.OnCraftPotionButtonClicked -= HandleCraftPotionButtonClicked;
    }

    private void HandleCraftPotionButtonClicked(List<MaterialType> materialTypeList)
    {
        Show();
        PotionData craftedPotion = FindMatchingPotion(materialTypeList);

        if (craftedPotion != null)
        {
            DisplayPotionSuccess(craftedPotion);
        }
        else
        {
            DisplayPotionFailure();
        }
    }

    private PotionData FindMatchingPotion(List<MaterialType> materialTypeList)
    {
        return potionDatabaseSO.PotionDataList.FirstOrDefault(potion =>
            potion.MaterialRecipes.Count == materialTypeList.Count &&
            !potion.MaterialRecipes.Except(materialTypeList).Any() &&
            !materialTypeList.Except(potion.MaterialRecipes).Any());
    }

    private void DisplayPotionSuccess(PotionData craftedPotion)
    {
        // Set potion details
        potionImage.sprite = craftedPotion.Sprite;
        potionName.text = craftedPotion.Name;
        descriptionText.text = "Potion Obtained!";

        // Reset and set stars based on rarity
        foreach (var starImage in potionStarImages)
        {
            starImage.sprite = gameAssetSO.InActivePotionStar;
            starImage.gameObject.SetActive(false);
        }

        for (int i = 0; i <= (int)craftedPotion.Rarity; i++)
        {
            potionStarImages[i].sprite = gameAssetSO.ActivePotionStar;
            potionStarImages[i].gameObject.SetActive(true);
        }
    }

    private void DisplayPotionFailure()
    {
        // Set failure feedback
        potionImage.sprite = gameAssetSO.FailedCraftedPotion;
        potionName.text = "Unknown Potion";
        descriptionText.text = "Crafting Failed!";

        // Reset stars
        foreach (var starImage in potionStarImages)
        {
            starImage.sprite = gameAssetSO.InActivePotionStar;
            starImage.gameObject.SetActive(false);
        }
    }

    private void HandleClose()
    {
        Hide();
    }
}
