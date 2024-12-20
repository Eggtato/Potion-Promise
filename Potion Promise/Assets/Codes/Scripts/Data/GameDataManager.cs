using System;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public int CurrentDay;
    public Action OnAllDataLoaded;
    public Action<int> OnCurrentDayChanged;
    public List<ObtainedMaterialData> ObtainedMaterialDataList;

    private GameData gameData;

    public new void Awake()
    {
        base.Awake();

        gameData = Data.Get<GameData>();

        CurrentDay = gameData.CurrentDay;
        ObtainedMaterialDataList = gameData.ObtainedMaterialDataList;
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

}
