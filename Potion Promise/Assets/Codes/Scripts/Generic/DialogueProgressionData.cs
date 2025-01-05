using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressionData
{
    public int Day;
    public List<ProgressionType> ProgressionTypes = new List<ProgressionType> { ProgressionType.Shop, ProgressionType.Gathering };
}

public enum ProgressionType
{
    EarlyVisualNovel,
    Shop,
    MiddleVisualNovel,
    Gathering,
    EndVisualNovel,
}
