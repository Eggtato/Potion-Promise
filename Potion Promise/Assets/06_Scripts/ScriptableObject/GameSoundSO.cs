using UnityEngine;

[CreateAssetMenu(fileName = "GameSoundSO", menuName = "Eggtato/Game Sound")]
public class GameSoundSO : ScriptableObject
{
    public AudioClip ClickSound;
    public AudioClip TypeSound;
    public AudioClip CoinSound;
    public AudioClip DayStartSound;
    public AudioClip DayEndSound;
    public AudioClip StirGrabSound;
    public AudioClip PountGrabSound;
    public AudioClip MaterialGrabSound;
    public AudioClip MaterialReleaseSound;
    public AudioClip TrashBinSound;
    public AudioClip[] MaterialFallsIntoCauldronSounds;
    public AudioClip[] MaterialStirredInCauldronSounds;
    public AudioClip[] MaterialSmashedSounds;
    public AudioClip CustomerComeSound;
}
