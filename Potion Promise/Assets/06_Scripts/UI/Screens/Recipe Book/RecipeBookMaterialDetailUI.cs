using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeBookMaterialDetailUI : MonoBehaviour
{
    [SerializeField] private Image materialImage;
    [SerializeField] private TMP_Text materialName;
    [SerializeField] private TMP_Text materialDescription;
    [SerializeField] private TMP_Text materialProfit;
    [SerializeField] private Image craftablePotionIconTemplate;
    [SerializeField] private Transform craftablePotionLayout;

    private PotionDatabaseSO potionDatabaseSO;
    private PlayerEventSO playerEventSO;
    private Dictionary<PotionType, Sprite> potionSpriteLookup = new Dictionary<PotionType, Sprite>();
    private RecipeBookMaterialPageUI recipeBookMaterialPageUI;

    private void Awake()
    {
        recipeBookMaterialPageUI = GetComponentInParent<RecipeBookMaterialPageUI>();
        if (recipeBookMaterialPageUI == null)
        {
            Debug.LogError("RecipeBookUI is missing in the parent hierarchy.");
            return;
        }
    }

    private void Start()
    {
        playerEventSO = recipeBookMaterialPageUI.PlayerEventSO;
        potionDatabaseSO = recipeBookMaterialPageUI.PotionDatabaseSO;

        BuildPotionSpriteLookup();

        HideAllUI();
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
            if (!potionSpriteLookup.ContainsKey(potionData.PotionType))
            {
                potionSpriteLookup[potionData.PotionType] = potionData.Sprite;
            }
        }
    }

    private void HideAllUI()
    {
        materialImage.gameObject.SetActive(false);
        materialName.gameObject.SetActive(false);
        materialDescription.gameObject.SetActive(false);
        materialProfit.gameObject.SetActive(false);
        craftablePotionIconTemplate.gameObject.SetActive(false);
    }

    private void ShowMaterialDetails(MaterialData materialData)
    {
        materialImage.gameObject.SetActive(true);
        materialImage.sprite = materialData.Sprite;

        materialName.gameObject.SetActive(true);
        materialName.text = materialData.Name;

        materialDescription.gameObject.SetActive(true);
        materialDescription.text = materialData.Description;

        materialProfit.gameObject.SetActive(true);
        materialProfit.text = $"<sprite name=coin> {materialData.Price}";


    }
}
