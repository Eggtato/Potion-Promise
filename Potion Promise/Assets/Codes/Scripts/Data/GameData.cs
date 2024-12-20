using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public int CurrentDay = 0;
    public int Coin = 0;
    public List<ObtainedMaterialData> ObtainedMaterialDataList = new List<ObtainedMaterialData>();
}
