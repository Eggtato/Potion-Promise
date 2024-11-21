using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


[System.Serializable]
public class PotionData
{
    [PreviewField(60), HideLabel][HorizontalGroup("Split", 60)] public Sprite Sprite;
    [VerticalGroup("Split/Right")] public string Name;
    [VerticalGroup("Split/Right")][EnumPaging] public PotionType PotionType;
    [VerticalGroup("Split/Right")][EnumPaging] public Rarity Rarity;
    [VerticalGroup("Split/Right")] public List<MaterialType> MaterialRecipes;
    [VerticalGroup("Split/Right")][Range(1, 50)] public int Price;
    [VerticalGroup("Split/Right")][Multiline(5)] public string Description;
}