using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


[System.Serializable]
public class PotionData
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 50)] public Sprite Sprite;
    public string Name;
    [EnumPaging] public PotionType PotionType;
    [EnumPaging] public Rarity Rarity;
    public float Price;
    public List<MaterialType> MaterialRecipes;
    [Multiline(5)] public string Description;
}