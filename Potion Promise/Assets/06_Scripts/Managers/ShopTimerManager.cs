using UnityEngine;

public class ShopTimerManager : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private GameSettingSO initialGameValueSO;

    private float timer;
    private bool isTimerFinished = true;

    private void Start()
    {
        timer = initialGameValueSO.ShopDuration;
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnOpenShopButtonClicked += StartTimer;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnOpenShopButtonClicked -= StartTimer;
    }

    private void StartTimer()
    {
        isTimerFinished = false;
    }

    private void Update()
    {
        if (isTimerFinished) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            playerEventSO.Event.OnDayEnd?.Invoke();
            isTimerFinished = true;
        }
    }
}
