using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class MaterialData
{
    [PreviewField(60), HideLabel][HorizontalGroup("Split", 60)] public Sprite Sprite;
    [VerticalGroup("Split/Right")] public string Name;
    [VerticalGroup("Split/Right")][EnumPaging] public MaterialType MaterialType;
    [VerticalGroup("Split/Right")][EnumPaging] public Rarity Rarity;
    [VerticalGroup("Split/Right")][Range(1, 10)] public int Price;
    [VerticalGroup("Split/Right")][Multiline(5)] public string Description;
    [VerticalGroup("Split/Right")] public Color32 Color;
}