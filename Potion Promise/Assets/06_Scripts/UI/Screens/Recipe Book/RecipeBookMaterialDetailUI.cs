using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RecipeBookMaterialDetailUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image materialImage;
    [SerializeField] private TMP_Text materialName;
    [SerializeField] private TMP_Text materialDescription;
    [SerializeField] private TMP_Text materialProfit;
    [SerializeField] private Image craftablePotionIcon;
    [SerializeField] private Transform craftablePotionPanel;
    [SerializeField] private Transform craftablePotionLayout;

    private PotionDatabaseSO potionDatabaseSO;
    private PlayerEventSO playerEventSO;
    private Dictionary<PotionType, Sprite> potionSpriteLookup;

    private RecipeBookMaterialPageUI recipeBookMaterialPageUI;

    private void Awake()
    {
        recipeBookMaterialPageUI = GetComponentInParent<RecipeBookMaterialPageUI>();
    }

    private void Start()
    {
        playerEventSO = recipeBookMaterialPageUI?.PlayerEventSO;
        potionDatabaseSO = recipeBookMaterialPageUI?.PotionDatabaseSO;

        if (potionDatabaseSO != null)
        {
            BuildPotionSpriteLookup();
        }

        HideAllUI();
        craftablePotionIcon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnMaterialSlotClicked += HandleOnMaterialSlotClicked;
        }
    }

    private void OnDisable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnMaterialSlotClicked -= HandleOnMaterialSlotClicked;
        }
    }

    private void HandleOnMaterialSlotClicked(MaterialData materialData)
    {
        if (materialData == null)
        {
            Debug.LogWarning("Material Data is null. Cannot display details.");
            return;
        }

        ShowMaterialDetails(materialData);
    }

    private void BuildPotionSpriteLookup()
    {
        potionSpriteLookup = new Dictionary<PotionType, Sprite>();
        foreach (var potionData in potionDatabaseSO.PotionDataList)
        {
            potionSpriteLookup[potionData.PotionType] = potionData.Sprite;
        }
    }

    private void HideAllUI()
    {
        SetUIActive(false);
    }

    private void SetUIActive(bool state)
    {
        materialImage.gameObject.SetActive(state);
        materialName.gameObject.SetActive(state);
        materialDescription.gameObject.SetActive(state);
        materialProfit.gameObject.SetActive(state);
        craftablePotionPanel.gameObject.SetActive(state);
    }

    private void ShowMaterialDetails(MaterialData materialData)
    {
        // Set material visuals
        materialImage.sprite = materialData.Sprite;
        materialName.text = materialData.Name;
        materialDescription.text = materialData.Description;
        materialProfit.text = $"<sprite name=coin> {materialData.Price}";

        SetUIActive(true);

        // Clear old icons except template
        foreach (Transform child in craftablePotionLayout)
        {
            if (child == craftablePotionIcon.transform) continue;
            Destroy(child.gameObject);
        }

        var relatedPotions = potionDatabaseSO.PotionDataList
            .Where(potion => potion.MaterialRecipes.Contains(materialData.MaterialType));

        foreach (var potion in relatedPotions)
        {
            var potionImage = Instantiate(craftablePotionIcon, craftablePotionLayout);
            potionImage.sprite = potion.Sprite;
            potionImage.gameObject.SetActive(true);
        }
    }
}
