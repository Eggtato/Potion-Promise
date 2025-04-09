using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopCraftingRoomUI : BaseUI
{
    [Header("UI")]
    [SerializeField] private List<Image> craftedMaterialImages = new List<Image>();

    private List<MaterialData> craftedMaterialDataList = new List<MaterialData>();
    private ShopCraftingManager shopCraftingManager;

    private void Start()
    {
        // Hide crafted material images initially
        foreach (var craftedImage in craftedMaterialImages)
        {
            craftedImage.gameObject.SetActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnAlchemyRoomOpened += HandleAlchemyRoomOpened;
            playerEventSO.Event.OnMaterialGetInCauldron += HandleMaterialAdded;
            playerEventSO.Event.OnCauldronStirred += HandlePotionCrafted;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnAlchemyRoomOpened -= HandleAlchemyRoomOpened;
            playerEventSO.Event.OnMaterialGetInCauldron -= HandleMaterialAdded;
            playerEventSO.Event.OnCauldronStirred -= HandlePotionCrafted;
        }
    }

    public void Initialize(ShopCraftingManager shopCraftingManager)
    {
        this.shopCraftingManager = shopCraftingManager;
    }

    private void HandlePotionCrafted()
    {
        List<MaterialType> materialTypeList = new List<MaterialType>();
        foreach (var item in craftedMaterialDataList)
        {
            materialTypeList.Add(item.MaterialType);
        }

        shopCraftingManager.HandlePotionCrafted(materialTypeList);

        craftedMaterialDataList.Clear();
        foreach (var item in craftedMaterialImages)
        {
            item.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles the event when the Alchemy Room is opened.
    /// </summary>
    private void HandleAlchemyRoomOpened()
    {
        Show();
    }

    /// <summary>
    /// Handles the event when a material is crafted.
    /// </summary>
    /// <param name="materialData">Data of the crafted material.</param>
    private void HandleMaterialAdded(MaterialData materialData)
    {
        if (materialData == null)
        {
            Debug.LogWarning("Crafted material data is null.");
            return;
        }

        if (craftedMaterialDataList.Count >= craftedMaterialImages.Count)
        {
            Debug.LogWarning("Crafted material limit reached. Cannot add more.");
            return;
        }

        craftedMaterialDataList.Add(materialData);

        // Update crafted material images
        var craftedImage = craftedMaterialImages[craftedMaterialDataList.Count - 1];
        craftedImage.gameObject.SetActive(true);
        craftedImage.sprite = materialData.Sprite;
    }
}
