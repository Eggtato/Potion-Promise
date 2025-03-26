using System;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public event Action OnAllDataLoaded;
    public event Action<int> OnCurrentDayChanged;

    private GameData gameData;
    private ProgressionSavedData progressionData;

    public int CurrentDay
    {
        get => gameData.CurrentDay;
        private set
        {
            gameData.CurrentDay = value;
            OnCurrentDayChanged?.Invoke(gameData.CurrentDay);
            SaveGameData();
        }
    }

    public List<ObtainedMaterialData> ObtainedMaterialDataList => gameData?.ObtainedMaterialDataList ?? new List<ObtainedMaterialData>();
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

        SaveGameData();
    }

    public void RemoveObtainedMaterialByOne(MaterialData materialData)
    {
        var data = gameData.ObtainedMaterialDataList.Find(i => i.MaterialType == materialData.MaterialType);
        if (data == null)
        {
            Debug.LogError("Material to be used couldn't be found");
        }
        else
        {
            data.Quantity--;
        }

        SaveGameData();
    }
}
