using UnityEngine;
using UnityEngine.UI;

public class RecipeBookUI : BaseUI
{
    [Header("Project References")]
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private RecipeSlotUI recipeSlotUI;

    [Header("Scene References")]
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private Button closeButton;

    public GameAssetSO GameAssetSO => gameAssetSO;

    private void Awake()
    {
        closeButton.onClick.AddListener(HandleOnClose);
    }

    private void Start()
    {
        // Ensure the inventory slot template is inactive by default
        if (recipeSlotUI != null)
            recipeSlotUI.gameObject.SetActive(false);

        InstantHide();
    }

    protected override void OnEnable()
    {
        playerEventSO.Event.OnRecipeBookOpened += GenerateInventorySlot;
    }

    protected override void OnDisable()
    {
        playerEventSO.Event.OnRecipeBookOpened -= GenerateInventorySlot;
    }

    public void GenerateInventorySlot()
    {
        Show();

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
                    slot.Initialize(item, gameAssetSO.ActivePotionStar, gameAssetSO.PotionCommmonCard);
                    break;
                case 1:
                    slot.Initialize(item, gameAssetSO.ActivePotionStar, gameAssetSO.PotionRareCard);
                    break;
                case 2:
                    slot.Initialize(item, gameAssetSO.ActivePotionStar, gameAssetSO.PotionEpicCard);
                    break;
            }
        }
    }

    private void HandleOnClose()
    {
        Hide();
    }
}
