using Eggtato.Utility;
using UnityEngine;

public class GameLevelManager : Singleton<GameLevelManager>
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    public int EarnedCoin = 0;

    void OnEnable()
    {
        playerEventSO.Event.OnDayEnd += HandleDayEnd;
    }

    void OnDisable()
    {
        playerEventSO.Event.OnDayEnd -= HandleDayEnd;
    }

    public void AddEarnedCoin(int amount)
    {
        EarnedCoin += amount;
        playerEventSO.Event.OnEarnedCoinChanged?.Invoke();
    }

    public void HandleDayEnd()
    {
        GameDataManager.Instance.PayDebt(EarnedCoin);
    }

}
