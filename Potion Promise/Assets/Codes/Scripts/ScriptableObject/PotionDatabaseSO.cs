using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PotionDatabaseSO", menuName = "Eggtato/Potion Database")]
public class PotionDatabaseSO : ScriptableObject
{
    public List<PotionData> PotionDataList = new List<PotionData>();
}
