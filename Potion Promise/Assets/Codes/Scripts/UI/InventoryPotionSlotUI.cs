using TMPro;
using UnityEngine;

public class InventoryPotionSlotUI : MonoBehaviour
{
    [SerializeField] private InventoryPotionImageUI inventoryPotionImageUI;
    [SerializeField] private TMP_Text quantityText;
    public CraftedPotionData craftedPotionData;

    public void Initialize(CraftedPotionData craftedPotionData, PotionData potionData)
    {
        this.craftedPotionData = craftedPotionData;
        inventoryPotionImageUI.Initialize(potionData);
        quantityText.text = "x" + craftedPotionData.Quantity;
    }
}
