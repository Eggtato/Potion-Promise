using System.Collections;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class GameLevelManager : Singleton<GameLevelManager>
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private GameSettingSO initialGameValueSO;

    [Header("UI Reference")]
    [SerializeField] private BaseUI dayEndUI;

    public int EarnedCoin = 0;
    public GameData TemporaryGameData;

    public List<ObtainedMaterialData> obtainedMaterialDatas = new List<ObtainedMaterialData>();
    public List<CraftedPotionData> craftedPotionDatas = new List<CraftedPotionData>();

    private void Start()
    {
        TemporaryGameData = GameDataManager.Instance.GetTemporaryCopy();
    }

    void OnEnable()
    {
        playerEventSO.Event.OnDayEnd += HandleDayEnd;
    }

    void OnDisable()
    {
        playerEventSO.Event.OnDayEnd -= HandleDayEnd;
    }

    public void AddObtainedMaterial(MaterialData materialData)
    {
        var data = TemporaryGameData.ObtainedMaterialDataList.Find(i => i.MaterialType == materialData.MaterialType);
        if (data == null)
        {
            data = new ObtainedMaterialData { MaterialType = materialData.MaterialType, Quantity = 1 };
            TemporaryGameData.ObtainedMaterialDataList.Add(data);
        }
        else
        {
            data.Quantity++;
        }

        playerEventSO.Event.OnMaterialInventoryChanged?.Invoke();
    }

    public void RemoveObtainedMaterialByOne(MaterialData materialData)
    {
        var data = TemporaryGameData.ObtainedMaterialDataList.Find(i => i.MaterialType == materialData.MaterialType);
        if (data != null)
        {
            data.Quantity--;
            if (data.Quantity <= 0)
            {
                TemporaryGameData.ObtainedMaterialDataList.Remove(data);
            }
        }

        playerEventSO.Event.OnMaterialInventoryChanged?.Invoke();
    }

    public void AddCraftedPotion(PotionType potionType)
    {
        var data = TemporaryGameData.CraftedPotionDataList.Find(i => i.PotionType == potionType);
        if (data == null)
        {
            data = new CraftedPotionData { PotionType = potionType, Quantity = 1 };
            TemporaryGameData.CraftedPotionDataList.Add(data);
        }
        else
        {
            data.Quantity++;
        }

        playerEventSO.Event.OnPotionInventoryChanged?.Invoke();
    }

    public void RemoveCraftedPotionByOne(PotionType potionType)
    {
        var data = TemporaryGameData.CraftedPotionDataList.Find(i => i.PotionType == potionType);
        if (data != null)
        {
            data.Quantity--;
            if (data.Quantity <= 0)
            {
                TemporaryGameData.CraftedPotionDataList.Remove(data);
            }
        }

        playerEventSO.Event.OnPotionInventoryChanged?.Invoke();
    }

    public void AddEarnedCoin(int amount)
    {
        EarnedCoin += amount;
        playerEventSO.Event.OnCoinEarned?.Invoke(amount);
        playerEventSO.Event.OnEarnedCoinChanged?.Invoke();
    }

    public void AddSoldPotion(PotionType potionType)
    {
        var temporaryData = craftedPotionDatas.Find(i => i.PotionType == potionType);
        if (temporaryData == null)
        {
            temporaryData = new CraftedPotionData { PotionType = potionType, Quantity = 1 };
            craftedPotionDatas.Add(temporaryData);
        }
        else
        {
            temporaryData.Quantity++;
        }
    }

    private void HandleDayEnd()
    {
        StartCoroutine(ProcessDayEnd());
    }

    private IEnumerator ProcessDayEnd()
    {
        if (dayEndUI) dayEndUI.Show();

        if (CrossSceneMessage.GetProgressionType(GameDataManager.Instance.CurrentDay.ToString()) == ProgressionType.Shop)
        {
            // Play end-of-day sound
            AudioManager.Instance.PlayShopDayEndSound();

            // Wait until UI finishes conversion
            ShopDayEndUI shopDayEndUI = dayEndUI as ShopDayEndUI;
            yield return shopDayEndUI.StartDebtConversion(EarnedCoin, TemporaryGameData.Debt);
        }

        // Apply debt payment and finalize the day
        PayDebt();
        FinalizeDay();
    }

    private void PayDebt()
    {
        if (EarnedCoin > 0) // Only reduce debt if there's an income
        {
            TemporaryGameData.Debt -= EarnedCoin;
            GameDataManager.Instance.UpdateDebt(TemporaryGameData.Debt);
        }
    }

    private void FinalizeDay()
    {
        // Save game state after UI and debt processing
        GameDataManager.Instance.UpdateObtainedMaterial(TemporaryGameData.ObtainedMaterialDataList);
        GameDataManager.Instance.UpdateCraftedPotion(TemporaryGameData.CraftedPotionDataList);
    }

}
