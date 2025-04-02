using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingSO", menuName = "Eggtato/Game Setting")]
public class GameSettingSO : ScriptableObject
{
    public GameData InitialGameValue;

    [Header("Game Setting")]
    [Unit(Units.Second)] public float ShopDuration;

    [Header("UI Animation")]
    [Unit(Units.Second)] public float FadeInAnimation;
    [Unit(Units.Second)] public float FadeOutAnimation;
}
