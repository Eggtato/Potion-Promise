using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "PotionDatabaseSO", menuName = "Eggtato/Potion Database")]
public class PotionDatabaseSO : ScriptableObject
{
    public List<PotionData> PotionDataList = new List<PotionData>();

    private Dictionary<PotionType, PotionData> potionLookup;

    private void OnEnable()
    {
        potionLookup = PotionDataList.ToDictionary(p => p.PotionType, p => p);
    }

    public PotionData GetPotionData(PotionType type)
    {
        return potionLookup.TryGetValue(type, out var potionData) ? potionData : null;
    }
}
