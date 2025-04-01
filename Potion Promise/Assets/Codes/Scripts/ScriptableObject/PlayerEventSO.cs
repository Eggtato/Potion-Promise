using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerEventSO", menuName = "Eggtato/Player Event")]
public class PlayerEventSO : ScriptableObject
{
    public PlayerEvent Event;

    [Button]
    public void TestDayEnd()
    {
        Event.OnDayEnd?.Invoke();
    }
}
