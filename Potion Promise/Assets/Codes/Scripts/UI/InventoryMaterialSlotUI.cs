using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryMaterialSlotUI : MonoBehaviour
{
    [SerializeField] private InventoryMaterialImageUI inventoryMaterialImageUI;
    [SerializeField] private TMP_Text quantityText;
    public ObtainedMaterialData obtainedMaterialData;

    public void Initialize(ObtainedMaterialData obtainedMaterialData, MaterialData MaterialData)
    {
        this.obtainedMaterialData = obtainedMaterialData;
        inventoryMaterialImageUI.Initialize(MaterialData);
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }

    public InventoryMaterialImageUI InitializeInGathering(ObtainedMaterialData obtainedMaterialData, MaterialData MaterialData, RewardManager rewardManager, GameAssetSO gameAssetSO)
    {
        this.obtainedMaterialData = obtainedMaterialData;
        inventoryMaterialImageUI.InitializeInGathering(MaterialData, rewardManager, gameAssetSO);
        inventoryMaterialImageUI.inGathering = true;
        quantityText.text = "x" + obtainedMaterialData.Quantity;

        return inventoryMaterialImageUI;
    }

    public void AddQuantity()
    {
        obtainedMaterialData.Quantity += 1;
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }
}
