using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using ModelShark;

public class InventoryMaterialSlotUI : MonoBehaviour, IResettableSlot
{
    [SerializeField] private InventoryMaterialImageUI inventoryMaterialImageUI;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TooltipTrigger tooltip;
    public ObtainedMaterialData obtainedMaterialData;

    public void Initialize(ObtainedMaterialData obtainedMaterialData, MaterialData MaterialData)
    {
        this.obtainedMaterialData = obtainedMaterialData;
        inventoryMaterialImageUI.Initialize(MaterialData);
        quantityText.text = "x" + obtainedMaterialData.Quantity;

        if (tooltip) tooltip.SetText("BodyText", MaterialData.Name);
    }

    public void AddQuantity()
    {
        obtainedMaterialData.Quantity += 1;
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }

    public void ResetSlot()
    {
        gameObject.SetActive(false);
    }
}
