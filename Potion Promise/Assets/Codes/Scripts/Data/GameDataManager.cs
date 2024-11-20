using System;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public Action OnAllDataLoaded;
    public List<ObtainedMaterialData> ObtainedMaterialDataList;

    public new void Awake()
    {
        base.Awake();

        var gameData = Data.Get<GameData>();

        ObtainedMaterialDataList = gameData.ObtainedMaterialDataList;
    }

    public void Save()
    {
        Data.Save();
    }

}
