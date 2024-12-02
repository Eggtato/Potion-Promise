using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameAssetSO", menuName = "Eggtato/Game Asset")]
public class GameAssetSO : ScriptableObject
{
    [PreviewField(60)] public List<Sprite> SmashedMaterialSprites = new List<Sprite>();
    [PreviewField(60)] public Sprite ActivePotionStar;
    [PreviewField(60)] public Sprite InActivePotionStar;
    [PreviewField(60)] public Sprite PotionCommmonCard;
    [PreviewField(60)] public Sprite PotionRareCard;
    [PreviewField(60)] public Sprite PotionEpicCard;
    [PreviewField(60)] public Sprite FailedCraftedPotion;
}
