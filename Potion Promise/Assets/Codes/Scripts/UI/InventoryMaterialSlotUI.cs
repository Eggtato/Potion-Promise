using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryMaterialSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    public void Initialize(ObtainedMaterialData obtainedMaterialData, Sprite obtainedMaterialSprite)
    {
        icon.sprite = obtainedMaterialSprite;
        quantityText.text = "x" + obtainedMaterialData.Quantity;
    }
}
