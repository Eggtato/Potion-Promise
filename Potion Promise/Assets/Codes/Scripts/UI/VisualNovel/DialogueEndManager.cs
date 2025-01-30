using UnityEngine;

public class DialogueEndManager : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;

    public void GoToNextScene()
    {
        playerEventSO.Event.OnGoToNextScene?.Invoke();
    }
}
