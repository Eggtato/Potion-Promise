using System;
using System.Collections.Generic;
using System.Linq;
using Eggtato.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public event Action OnAllDataLoaded;

    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private GameSettingSO initialGameValueSO;
    [SerializeField] private DayProgressionSO dayProgressionSO;

    private GameData gameData;
    private ProgressionSavedData progressionData;

    public int CurrentDay
    {
        get => gameData.CurrentDay;
        private set
        {
            gameData.CurrentDay = value;
            playerEventSO.Event.OnCurrentDayChanged?.Invoke();
            SaveGameData();
        }
    }

    public int Debt
    {
        get => gameData?.Debt ?? initialGameValueSO.InitialGameValue.Debt;
        private set
        {
            gameData.Debt = value;
            SaveGameData();
        }
    }
    public List<ObtainedMaterialData> ObtainedMaterialDataList => gameData?.ObtainedMaterialDataList ?? new List<ObtainedMaterialData>();
    public List<CraftedPotionData> CraftedPotionDataList => gameData?.CraftedPotionDataList ?? new List<CraftedPotionData>();
    public List<ProgressionData> ProgressionDataList => progressionData?.ProgressionDataList ?? new List<ProgressionData>();

    public new void Awake()
    {
        base.Awake();
        LoadData();
    }

    private void LoadData()
    {
        gameData = Data.Get<GameData>() ?? new GameData();
        progressionData = Data.Get<ProgressionSavedData>() ?? new ProgressionSavedData();

        OnAllDataLoaded?.Invoke();
    }

    public void SaveGameData() => Data.Save<GameData>();
    public void SaveProgressionData() => Data.Save<ProgressionSavedData>();

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        LoadData();
        SaveGameData();
        SaveProgressionData();
    }

    public GameData GetTemporaryCopy()
    {
        return gameData.Clone(); // Get a copy for temporary modification
    }

    public void IncreaseCurrentDay()
    {
        CurrentDay++;
    }

    public void AddNewProgression(int day, ProgressionType progressionType)
    {
        var data = progressionData.ProgressionDataList.Find(i => i.Day == day);
        if (data == null)
        {
            data = new ProgressionData { Day = day, ProgressionTypes = new List<ProgressionType> { progressionType } };
            progressionData.ProgressionDataList.Add(data);
        }
        else if (!data.ProgressionTypes.Contains(progressionType))
        {
            data.ProgressionTypes.Add(progressionType);
        }

        SaveProgressionData();
    }

    public ProgressionType GetCurrentProgressionType()
    {
        int currentDay = CurrentDay;

        // Get the saved progression data for the current day
        ProgressionData savedData = ProgressionDataList
            .FirstOrDefault(i => i.Day == currentDay);

        if (savedData == null || savedData.ProgressionTypes.Count == 0)
        {
            Debug.LogWarning($"No saved progression found for day {currentDay}, starting from first progression.");
            return dayProgressionSO.DayProgressionDataList
                .FirstOrDefault(i => i.Day == currentDay)?.ProgressionTypes.FirstOrDefault()
                ?? throw new Exception($"No progression data found for day {currentDay}");
        }

        // Return the last progression type recorded for the current day
        return savedData.ProgressionTypes.Last();
    }

    public bool IsCurrentSceneProgression()
    {
        return SceneManager.GetActiveScene().name != "MainMenu"; // Adjust based on your scene names
    }

    public void UpdateDebt(int amount)
    {
        Debt = amount;
    }

    public void UpdateObtainedMaterial(List<ObtainedMaterialData> obtainedMaterialDataList)
    {
        gameData.ObtainedMaterialDataList = obtainedMaterialDataList;
        SaveGameData();
    }

    public void AddObtainedMaterial(MaterialData materialData)
    {
        var data = gameData.ObtainedMaterialDataList.Find(i => i.MaterialType == materialData.MaterialType);
        if (data == null)
        {
            data = new ObtainedMaterialData { MaterialType = materialData.MaterialType, Quantity = 1 };
            gameData.ObtainedMaterialDataList.Add(data);
        }
        else
        {
            data.Quantity++;
        }

        playerEventSO.Event.OnMaterialInventoryChanged?.Invoke();
        SaveGameData();
    }

    public void RemoveObtainedMaterialByOne(MaterialData materialData)
    {
        var data = gameData.ObtainedMaterialDataList.Find(i => i.MaterialType == materialData.MaterialType);
        if (data != null)
        {
            data.Quantity--;
            if (data.Quantity <= 0)
            {
                gameData.ObtainedMaterialDataList.Remove(data);
            }
        }

        playerEventSO.Event.OnMaterialInventoryChanged?.Invoke();
        SaveGameData();
    }

    public void UpdateCraftedPotion(List<CraftedPotionData> craftedPotionDataList)
    {
        gameData.CraftedPotionDataList = craftedPotionDataList;
        SaveGameData();
    }

    public void AddCraftedPotion(PotionType potionType)
    {
        var data = gameData.CraftedPotionDataList.Find(i => i.PotionType == potionType);
        if (data == null)
        {
            data = new CraftedPotionData { PotionType = potionType, Quantity = 1 };
            gameData.CraftedPotionDataList.Add(data);
        }
        else
        {
            data.Quantity++;
        }

        playerEventSO.Event.OnPotionInventoryChanged?.Invoke();
        SaveGameData();
    }

    public void RemoveCraftedPotionByOne(PotionType potionType)
    {
        var data = gameData.CraftedPotionDataList.Find(i => i.PotionType == potionType);
        if (data != null)
        {
            data.Quantity--;
            if (data.Quantity <= 0)
            {
                gameData.CraftedPotionDataList.Remove(data);
            }
        }

        playerEventSO.Event.OnPotionInventoryChanged?.Invoke();
        SaveGameData();
    }
}
