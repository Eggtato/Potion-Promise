using TMPro;
using UnityEngine;

public class InventoryPotionSlotUI : MonoBehaviour, IResettableSlot
{
    [SerializeField] private InventoryPotionImageUI inventoryPotionImageUI;
    [SerializeField] private TMP_Text quantityText;
    public CraftedPotionData craftedPotionData;

    public void Initialize(CraftedPotionData craftedPotionData, PotionData potionData, bool isInteractable = true)
    {
        this.craftedPotionData = craftedPotionData;
        inventoryPotionImageUI.Initialize(potionData, isInteractable);
        quantityText.text = "x" + craftedPotionData.Quantity;
    }

    public void ResetSlot()
    {
        gameObject.SetActive(false);
    }
}
