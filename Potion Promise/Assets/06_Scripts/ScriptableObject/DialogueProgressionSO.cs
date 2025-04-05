using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayProgressionSO", menuName = "Eggtato/Day Progression")]
public class DayProgressionSO : ScriptableObject
{
    public List<ProgressionData> DayProgressionDataList = new List<ProgressionData>();
}
