using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameAssetSO", menuName = "Eggtato/Game Asset")]
public class GameAssetSO : ScriptableObject
{
    [PreviewField(60)] public List<Sprite> SmashedMaterialSprites = new List<Sprite>();
    [PreviewField(60)] public List<Sprite> StirredMaterialSprites = new List<Sprite>();
    [PreviewField(60)] public Sprite ActiveStar;
    [PreviewField(60)] public Sprite InActiveStar;

    [Header("Recipe Book / Potion Page")]
    [PreviewField(60)] public Sprite PotionCommmonCard;
    [PreviewField(60)] public Sprite SelectedPotionCommmonCard;
    [PreviewField(60)] public Sprite PotionRareCard;
    [PreviewField(60)] public Sprite SelectedPotionRareCard;
    [PreviewField(60)] public Sprite PotionEpicCard;
    [PreviewField(60)] public Sprite SelectedPotionEpicCard;

    [Header("Recipe Book / Material Page")]
    [PreviewField(60)] public Sprite MaterialCommmonCard;
    [PreviewField(60)] public Sprite SelectedMaterialCommmonCard;
    [PreviewField(60)] public Sprite MaterialRareCard;
    [PreviewField(60)] public Sprite SelectedMaterialRareCard;
    [PreviewField(60)] public Sprite MaterialEpicCard;
    [PreviewField(60)] public Sprite SelectedMaterialEpicCard;

    [PreviewField(60)] public Sprite FailedCraftedPotion;
}
