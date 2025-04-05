using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialDatabaseSO", menuName = "Eggtato/Material Database")]
public class MaterialDatabaseSO : ScriptableObject
{
    public List<MaterialData> MaterialDataList = new List<MaterialData>();
}
