using UnityEngine;

public class RecipeBookPotionInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private RecipeSlotUI recipeSlotUI;

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
            RecipeSlotUI slot = Instantiate(recipeSlotUI, inventoryContent);
            slot.gameObject.SetActive(true);

            switch ((int)item.Rarity)
            {
                case 0:
                    slot.Initialize(
                        item,
                        gameAssetSO.ActivePotionStar,
                        gameAssetSO.PotionCommmonCard,
                        gameAssetSO.SelectedPotionCommmonCard,
                        playerEventSO.Event.OnPotionSlotClicked
                    );
                    break;
                case 1:
                    slot.Initialize(
                        item,
                        gameAssetSO.ActivePotionStar,
                        gameAssetSO.PotionRareCard,
                        gameAssetSO.SelectedPotionRareCard,
                        playerEventSO.Event.OnPotionSlotClicked
                    );
                    break;
                case 2:
                    slot.Initialize(
                        item,
                        gameAssetSO.ActivePotionStar,
                        gameAssetSO.PotionEpicCard,
                        gameAssetSO.SelectedPotionEpicCard,
                        playerEventSO.Event.OnPotionSlotClicked
                    );
                    break;
            }
        }
    }
}
