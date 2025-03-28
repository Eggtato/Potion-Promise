using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public int CurrentDay = 1;
    public int Debt = 10000;
    public List<ObtainedMaterialData> ObtainedMaterialDataList = new List<ObtainedMaterialData>();
    public List<CraftedPotionData> CraftedPotionDataList = new List<CraftedPotionData>();
}

public class ProgressionSavedData : BaseData
{
    public override string Name => "Dialogue Progression Data";
    public override string Key => "Dialogue ProgressionData";

    public List<ProgressionData> ProgressionDataList = new List<ProgressionData>() { };
}
