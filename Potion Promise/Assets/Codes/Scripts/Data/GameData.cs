using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public int CurrentDay = 1;
    public int Coin = 0;
    public List<ObtainedMaterialData> ObtainedMaterialDataList = new List<ObtainedMaterialData>();
}

public class ProgressionSavedData : BaseData
{
    public override string Name => "Dialogue Progression Data";
    public override string Key => "Dialogue ProgressionData";

    public List<ProgressionData> ProgressionDataList = new List<ProgressionData>() { };
}
