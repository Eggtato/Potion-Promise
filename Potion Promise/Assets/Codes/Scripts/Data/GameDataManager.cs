using System;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public int CurrentDay;
    public Action OnAllDataLoaded;
    public Action<int> OnCurrentDayChanged;
    public List<ObtainedMaterialData> ObtainedMaterialDataList = new();
    public List<ProgressionData> ProgressionDataList = new List<ProgressionData>();

    private GameData gameData;
    private ProgressionSavedData progressionData;

    public new void Awake()
    {
        base.Awake();

        gameData = Data.Get<GameData>();
        progressionData = Data.Get<ProgressionSavedData>();

        CurrentDay = gameData.CurrentDay;
        ObtainedMaterialDataList = gameData.ObtainedMaterialDataList;
        ProgressionDataList = progressionData.ProgressionDataList;
    }

    public void Save()
    {
        Data.Save();
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        Save();
    }

    public void IncreaseCurrentDay()
    {
        gameData.CurrentDay++;
        CurrentDay = gameData.CurrentDay;
        OnCurrentDayChanged?.Invoke(CurrentDay);

        Save();
    }

    public void AddNewProgression(int day, ProgressionType progressionType)
    {
        var data = progressionData.ProgressionDataList.Find(i => i.Day == day);
        if (data == null)
        {
            ProgressionData newProgressionData = new()
            {
                Day = day
            };

            newProgressionData.ProgressionTypes.Clear();
            newProgressionData.ProgressionTypes.Add(progressionType);

            progressionData.ProgressionDataList.Add(newProgressionData);
        }
        else
        {
            data.ProgressionTypes.Add(progressionType);
        }

        progressionData.Save();
    }

}
