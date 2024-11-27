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
    [SerializeField] private Transform parent;
    [SerializeField] private List<Image> craftedMaterialImages = new List<Image>();

    private int currentCraftedMaterialCount = 0;
    private List<MaterialData> craftedMaterialDataList = new List<MaterialData>();

    private void Start()
    {
        inventorySlotTemplate.gameObject.SetActive(false);

        InitializeInventorySlots(GameDataManager.Instance.ObtainedMaterialDataList);

        foreach (var item in craftedMaterialImages)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void InitializeInventorySlots(List<ObtainedMaterialData> obtainedMaterialDataList)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<InventoryMaterialSlotUI>() == inventorySlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var item in obtainedMaterialDataList)
        {
            MaterialData materialData = materialDatabaseSO.MaterialDataList.First(i => i.MaterialType == item.MaterialType);
            var slotUI = Instantiate(inventorySlotTemplate, parent);
            slotUI.gameObject.SetActive(true);
            slotUI.Initialize(item, materialData);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnAlchemyRoomOpnened += HandleAlchemyRoomOpened;
        playerEventSO.Event.OnMaterialCrafted += HandleMaterialCrafted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnAlchemyRoomOpnened -= HandleAlchemyRoomOpened;
        playerEventSO.Event.OnMaterialCrafted -= HandleMaterialCrafted;
    }

    private void HandleAlchemyRoomOpened()
    {
        Show();
    }

    private void HandleMaterialCrafted(MaterialData materialData)
    {
        currentCraftedMaterialCount++;

        craftedMaterialDataList.Add(materialData);

        for (int i = 0; i < craftedMaterialDataList.Count; i++)
        {
            craftedMaterialImages[i].gameObject.SetActive(true);
            craftedMaterialImages[i].sprite = craftedMaterialDataList[i].Sprite;
        }
    }
}
