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
    }

    public void DisplayPotionFailure()
    {
        Show();

        // Set failure feedback
        potionImage.sprite = gameAssetSO.FailedCraftedPotion;
        potionName.text = "Unknown Potion";
        descriptionText.text = "Crafting Failed!";
    }

    private void HandleClose()
    {
        Hide();
    }
}
