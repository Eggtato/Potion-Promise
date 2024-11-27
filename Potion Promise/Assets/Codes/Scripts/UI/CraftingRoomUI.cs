using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRoomUI : BaseUI
{
    [Header("Database Reference")]
    [SerializeField] private MaterialDatabaseSO materialDatabaseSO;

    [Header("UI References")]
    [SerializeField] private InventoryMaterialSlotUI inventorySlotTemplate;
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private List<Image> craftedMaterialImages = new List<Image>();

    private List<MaterialData> craftedMaterialDataList = new List<MaterialData>();

    private void Start()
    {
        // Ensure the inventory slot template is inactive by default
        if (inventorySlotTemplate != null)
            inventorySlotTemplate.gameObject.SetActive(false);

        // Initialize inventory slots based on the obtained materials
        var obtainedMaterials = GameDataManager.Instance?.ObtainedMaterialDataList ?? new List<ObtainedMaterialData>();
        InitializeInventorySlots(obtainedMaterials);

        // Hide crafted material images initially
        foreach (var craftedImage in craftedMaterialImages)
        {
            craftedImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Initializes the inventory slots based on the obtained materials.
    /// </summary>
    /// <param name="obtainedMaterialDataList">List of obtained materials.</param>
    private void InitializeInventorySlots(List<ObtainedMaterialData> obtainedMaterialDataList)
    {
        // Clear existing slots except the template
        foreach (Transform child in inventoryParent)
        {
            if (child.gameObject == inventorySlotTemplate.gameObject) continue;
            Destroy(child.gameObject);
        }

        // Create a slot for each obtained material
        foreach (var obtainedMaterial in obtainedMaterialDataList)
        {
            var materialData = materialDatabaseSO.MaterialDataList
                .FirstOrDefault(m => m.MaterialType == obtainedMaterial.MaterialType);

            if (materialData == null)
            {
                Debug.LogWarning($"MaterialData not found for type: {obtainedMaterial.MaterialType}");
                continue;
            }

            var slotUI = Instantiate(inventorySlotTemplate, inventoryParent);
            slotUI.gameObject.SetActive(true);
            slotUI.Initialize(obtainedMaterial, materialData);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnAlchemyRoomOpened += HandleAlchemyRoomOpened;
            playerEventSO.Event.OnMaterialCrafted += HandleMaterialCrafted;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnAlchemyRoomOpened -= HandleAlchemyRoomOpened;
            playerEventSO.Event.OnMaterialCrafted -= HandleMaterialCrafted;
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
    private void HandleMaterialCrafted(MaterialData materialData)
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
