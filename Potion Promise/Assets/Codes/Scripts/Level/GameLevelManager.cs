using Eggtato.Utility;
using UnityEngine;

public class GameLevelManager : Singleton<GameLevelManager>
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [SerializeField] private ShopCustomerManager shopCustomerManager;
    [SerializeField] private int earnedCoin;

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
        earnedCoin += amount;
    }

    public void HandleDayEnd()
    {
        GameDataManager.Instance.PayDebt(earnedCoin);
    }

}
