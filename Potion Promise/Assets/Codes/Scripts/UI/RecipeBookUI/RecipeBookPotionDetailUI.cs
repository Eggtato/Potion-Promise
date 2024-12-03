using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookPotionDetailUI : MonoBehaviour
{
    [SerializeField] private Image potionImage;
    [SerializeField] private List<Image> potionMaterialImages = new();
    [SerializeField] private TMP_Text potionName;
    [SerializeField] private TMP_Text potionDescription;
    [SerializeField] private TMP_Text potionProfit;

    private MaterialDatabaseSO materialDatabaseSO;
    private PlayerEventSO playerEventSO;
    private Dictionary<MaterialType, Sprite> materialSpriteLookup = new Dictionary<MaterialType, Sprite>();
    private RecipeBookPotionPageUI recipeBookPotionPageUI;

    private void Awake()
    {
        recipeBookPotionPageUI = GetComponentInParent<RecipeBookPotionPageUI>();
        if (recipeBookPotionPageUI == null)
        {
            Debug.LogError("RecipeBookUI is missing in the parent hierarchy.");
            return;
        }
    }

    private void Start()
    {
        playerEventSO = recipeBookPotionPageUI.PlayerEventSO;
        materialDatabaseSO = recipeBookPotionPageUI.MaterialDatabaseSO;

        BuildMaterialSpriteLookup();

        HideAllUI();
    }

    private void OnEnable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnPotionSlotClicked += HandleOnPotionSlotClicked;
        }
    }

    private void OnDisable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnPotionSlotClicked -= HandleOnPotionSlotClicked;
        }
    }

    private void HandleOnPotionSlotClicked(PotionData potionData)
    {
        if (potionData == null)
        {
            Debug.LogWarning("PotionData is null. Cannot display details.");
            return;
        }

        ShowPotionDetails(potionData);
        UpdateMaterialImages(potionData.MaterialRecipes);
    }

    private void BuildMaterialSpriteLookup()
    {
        materialSpriteLookup = new Dictionary<MaterialType, Sprite>();

        foreach (var materialData in materialDatabaseSO.MaterialDataList)
        {
            if (!materialSpriteLookup.ContainsKey(materialData.MaterialType))
            {
                materialSpriteLookup[materialData.MaterialType] = materialData.Sprite;
            }
        }
    }

    private void HideAllUI()
    {
        potionImage.gameObject.SetActive(false);
        potionName.gameObject.SetActive(false);
        potionDescription.gameObject.SetActive(false);
        potionProfit.gameObject.SetActive(false);

        foreach (var image in potionMaterialImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    private void ShowPotionDetails(PotionData potionData)
    {
        potionImage.gameObject.SetActive(true);
        potionImage.sprite = potionData.Sprite;

        potionName.gameObject.SetActive(true);
        potionName.text = potionData.Name;

        potionDescription.gameObject.SetActive(true);
        potionDescription.text = potionData.Description;

        potionProfit.gameObject.SetActive(true);
        potionProfit.text = $"<sprite name=coin> {potionData.Price}";
    }

    private void UpdateMaterialImages(List<MaterialType> materialRecipes)
    {
        for (int i = 0; i < potionMaterialImages.Count; i++)
        {
            if (i < materialRecipes.Count && materialSpriteLookup.TryGetValue(materialRecipes[i], out var sprite))
            {
                potionMaterialImages[i].gameObject.SetActive(true);
                potionMaterialImages[i].sprite = sprite;
            }
            else
            {
                potionMaterialImages[i].gameObject.SetActive(false);
            }
        }
    }
}
