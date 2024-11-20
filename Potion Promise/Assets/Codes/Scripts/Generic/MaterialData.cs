using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class MaterialData
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 50)] public Sprite Sprite;
    public string Name;
    [EnumPaging] public MaterialType MaterialType;
    [EnumPaging] public Rarity Rarity;
    public int Price;
    [Multiline(5)] public string Description;
}