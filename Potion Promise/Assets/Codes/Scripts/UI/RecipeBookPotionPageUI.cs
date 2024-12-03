using UnityEngine;

public class RecipeBookPotionPageUI : BaseUI
{
    [SerializeField] private RecipeBookPotionInventoryUI potionInventoryUI;
    [SerializeField] private RecipeBookPotionDetailUI potionDetailUI;

    private PotionDatabaseSO potionDatabaseSO;
    private MaterialDatabaseSO materialDatabaseSO;
    private GameAssetSO gameAssetSO;

    public PlayerEventSO PlayerEventSO => playerEventSO;
    public MaterialDatabaseSO MaterialDatabaseSO => materialDatabaseSO;

    protected override void OnEnable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnPotionPageTabButtonClicked += ShowPage;
        }
    }

    protected override void OnDisable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnPotionPageTabButtonClicked -= ShowPage;
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
