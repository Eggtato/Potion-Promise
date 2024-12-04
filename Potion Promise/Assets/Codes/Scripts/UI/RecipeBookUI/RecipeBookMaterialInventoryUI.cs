using UnityEngine;

public class RecipeBookMaterialInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private MaterialRecipeSlotUI recipeSlotUI;

    private void Start()
    {
        // Ensure the inventory slot template is inactive by default
        if (recipeSlotUI != null)
            recipeSlotUI.gameObject.SetActive(false);
    }

    public void GenerateInventorySlot(MaterialDatabaseSO materialDatabaseSO, GameAssetSO gameAssetSO, PlayerEventSO playerEventSO)
    {
        // Clear existing slots except the template
        foreach (Transform child in inventoryContent)
        {
            if (child.gameObject == recipeSlotUI.gameObject) continue;
            Destroy(child.gameObject);
        }

        foreach (var item in materialDatabaseSO.MaterialDataList)
        {
            MaterialRecipeSlotUI slot = Instantiate(recipeSlotUI, inventoryContent);
            slot.gameObject.SetActive(true);
            slot.Initialize(item, playerEventSO, gameAssetSO);
        }
    }
}
