using UnityEngine;

public class RecipeBookMaterialInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private RecipeSlotUI recipeSlotUI;

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
            RecipeSlotUI slot = Instantiate(recipeSlotUI, inventoryContent);
            slot.gameObject.SetActive(true);

            switch ((int)item.Rarity)
            {
                case 0:
                    slot.Initialize(
                        item,
                        playerEventSO,
                        gameAssetSO.ActiveStar,
                        gameAssetSO.MaterialCommmonCard,
                        gameAssetSO.SelectedMaterialCommmonCard
                    );
                    break;
                case 1:
                    slot.Initialize(
                        item,
                        playerEventSO,
                        gameAssetSO.ActiveStar,
                        gameAssetSO.MaterialRareCard,
                        gameAssetSO.SelectedMaterialRareCard
                    );
                    break;
                case 2:
                    slot.Initialize(
                        item,
                        playerEventSO,
                        gameAssetSO.ActiveStar,
                        gameAssetSO.MaterialEpicCard,
                        gameAssetSO.SelectedMaterialEpicCard
                    );
                    break;
            }
        }
    }
}
