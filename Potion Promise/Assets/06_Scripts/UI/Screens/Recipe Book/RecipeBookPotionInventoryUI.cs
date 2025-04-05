using UnityEngine;

public class RecipeBookPotionInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private PotionRecipeSlotUI recipeSlotUI;

    private void Start()
    {
        // Ensure the inventory slot template is inactive by default
        if (recipeSlotUI != null)
            recipeSlotUI.gameObject.SetActive(false);
    }

    public void GenerateInventorySlot(PotionDatabaseSO potionDatabaseSO, GameAssetSO gameAssetSO, PlayerEventSO playerEventSO)
    {
        // Clear existing slots except the template
        foreach (Transform child in inventoryContent)
        {
            if (child.gameObject == recipeSlotUI.gameObject) continue;
            Destroy(child.gameObject);
        }

        foreach (var item in potionDatabaseSO.PotionDataList)
        {
            PotionRecipeSlotUI slot = Instantiate(recipeSlotUI, inventoryContent);
            slot.gameObject.SetActive(true);
            slot.Initialize(item, playerEventSO, gameAssetSO);
        }
    }
}
