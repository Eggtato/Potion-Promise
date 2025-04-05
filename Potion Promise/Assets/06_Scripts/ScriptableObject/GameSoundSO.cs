using UnityEngine;

[CreateAssetMenu(fileName = "GameSoundSO", menuName = "Eggtato/Game Sound")]
public class GameSoundSO : ScriptableObject
{
    public AudioClip ClickSound;
    public AudioClip TypeSound;
    public AudioClip CoinSound;
    public AudioClip DayEndSound;
}
