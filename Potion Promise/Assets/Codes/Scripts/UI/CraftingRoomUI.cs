using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingRoomUI : BaseUI
{
    [Header("Database Reference")]
    [SerializeField] private MaterialDatabaseSO materialDatabaseSO;

    [Header("UI References")]
    [SerializeField] private InventoryMaterialSlotUI inventorySlotTemplate;
    [SerializeField] private Transform parent;

    private void Start()
    {
        inventorySlotTemplate.gameObject.SetActive(false);

        InitializeInventorySlots(GameDataManager.Instance.ObtainedMaterialDataList);
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
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnAlchemyRoomOpnened -= HandleAlchemyRoomOpened;
    }

    private void HandleAlchemyRoomOpened()
    {
        Show();
    }
}
