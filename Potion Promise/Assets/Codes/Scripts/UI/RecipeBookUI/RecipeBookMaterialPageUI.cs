using UnityEngine;

public class RecipeBookMaterialPageUI : BaseUI
{
    [SerializeField] private RecipeBookMaterialInventoryUI materialInventoryUI;

    private PotionDatabaseSO potionDatabaseSO;
    private MaterialDatabaseSO materialDatabaseSO;
    private GameAssetSO gameAssetSO;

    public PlayerEventSO PlayerEventSO => playerEventSO;
    public MaterialDatabaseSO MaterialDatabaseSO => materialDatabaseSO;
    public PotionDatabaseSO PotionDatabaseSO => potionDatabaseSO;

    protected override void OnEnable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnRecipeBookPageInitialized += Initialize;
            playerEventSO.Event.OnMaterialTabButtonClicked += ShowPage;
            playerEventSO.Event.OnAnyPageUIClosed += InstantHide;
        }
    }

    protected override void OnDisable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnRecipeBookPageInitialized -= Initialize;
            playerEventSO.Event.OnMaterialTabButtonClicked -= ShowPage;
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
        materialInventoryUI.GenerateInventorySlot(materialDatabaseSO, gameAssetSO, playerEventSO);
    }
}
