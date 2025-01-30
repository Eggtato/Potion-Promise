using UnityEngine;

public class RecipeBookPotionPageUI : BaseUI
{
    [SerializeField] private RecipeBookPotionInventoryUI potionInventoryUI;

    private PotionDatabaseSO potionDatabaseSO;
    private MaterialDatabaseSO materialDatabaseSO;
    private GameAssetSO gameAssetSO;

    public MaterialDatabaseSO MaterialDatabaseSO => materialDatabaseSO;

    protected override void OnEnable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnRecipeBookPageInitialized += Initialize;
            playerEventSO.Event.OnPotionPageTabButtonClicked += ShowPage;
            playerEventSO.Event.OnAnyPageUIClosed += InstantHide;
        }
    }

    protected override void OnDisable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnRecipeBookPageInitialized -= Initialize;
            playerEventSO.Event.OnPotionPageTabButtonClicked -= ShowPage;
            playerEventSO.Event.OnAnyPageUIClosed -= InstantHide;
        }
    }

    public void Initialize(PotionDatabaseSO potionDatabaseSO, MaterialDatabaseSO materialDatabaseSO, GameAssetSO gameAssetSO)
    {
        this.potionDatabaseSO = potionDatabaseSO;
        this.materialDatabaseSO = materialDatabaseSO;
        this.gameAssetSO = gameAssetSO;

    }

    private void ShowPage()
    {
        Show();
        potionInventoryUI.GenerateInventorySlot(potionDatabaseSO, gameAssetSO, playerEventSO);
    }
}
