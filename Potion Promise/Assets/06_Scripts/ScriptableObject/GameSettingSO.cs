using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingSO", menuName = "Eggtato/Game Setting")]
public class GameSettingSO : ScriptableObject
{
    public GameData InitialGameValue;

    [Header("Game Setting")]
    [Unit(Units.Second)] public float ShopDuration;
    public int ShopCustomerLimitPerDay = 5;

    [Header("UI Animation")]
    [Unit(Units.Second)] public float CraftingMaterialFadeInAnimation = 0.5f;
    [Unit(Units.Second)] public float FadeInAnimation;
    [Unit(Units.Second)] public float FadeOutAnimation;
}
