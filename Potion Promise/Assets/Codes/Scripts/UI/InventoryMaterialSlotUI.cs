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

    public void Initialize(ObtainedMaterialData obtainedMaterialData, MaterialData materialData)
    {
        inventoryMaterialImageUI.Initialize(materialData);
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }
}
