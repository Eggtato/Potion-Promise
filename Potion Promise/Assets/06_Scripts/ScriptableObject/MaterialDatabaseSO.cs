using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialDatabaseSO", menuName = "Eggtato/Material Database")]
public class MaterialDatabaseSO : ScriptableObject
{
    public List<MaterialData> MaterialDataList = new List<MaterialData>();

    private Dictionary<MaterialType, MaterialData> materialLookup;

    private void OnEnable()
    {
        materialLookup = MaterialDataList.ToDictionary(p => p.MaterialType, p => p);
    }

    public MaterialData GetMaterialData(MaterialType type)
    {
        return materialLookup.TryGetValue(type, out var potionData) ? potionData : null;
    }
}
