using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryMaterialSlotUIGathering : MonoBehaviour
{
    public InventoryMaterialImageUIGathering inventoryMaterialImageUIGathering;
    [SerializeField] private TMP_Text quantityText;
    public ObtainedMaterialData obtainedMaterialData { get; private set; }
    public MaterialData materialData { get; private set; }


    public InventoryMaterialImageUIGathering Initialize(ObtainedMaterialData obtainedMaterialData, MaterialData materialData, RewardManagerUI rewardManager, GameAssetSO gameAssetSO)
    {
        this.obtainedMaterialData = obtainedMaterialData;
        this.materialData = materialData;
        inventoryMaterialImageUIGathering.Initialize(materialData, rewardManager, gameAssetSO);
        quantityText.text = "x" + obtainedMaterialData.Quantity;

        return inventoryMaterialImageUIGathering;
    }

    public void AddQuantity()
    {
        obtainedMaterialData.Quantity += 1;
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }
}
