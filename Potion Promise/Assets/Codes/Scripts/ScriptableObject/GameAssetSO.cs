using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameAssetSO", menuName = "Eggtato/Game Asset")]
public class GameAssetSO : ScriptableObject
{
    [PreviewField(60)] public List<Sprite> SmashedMaterialSprites = new List<Sprite>();
}
