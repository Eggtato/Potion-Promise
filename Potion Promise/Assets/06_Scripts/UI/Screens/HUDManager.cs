using Eggtato.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [Header("Pages")]
    [SerializeField] private ShopCustomerRoomUI customerRoomUI;

    [Header("Texts")]
    [SerializeField] private TMP_Text earnedMoneyText;
    [SerializeField] private TMP_Text currentDayText;

    [Header("Navigation")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button recipeBook;

    public new void Awake()
    {
        base.Awake();

        leftButton.onClick.AddListener(ShowCustomerRoom);
        rightButton.onClick.AddListener(ShowAlchemyRoom);
        recipeBook.onClick.AddListener(ShowRecipeBook);
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnEarnedCoinChanged += RefreshUI;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnEarnedCoinChanged -= RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
        SetDefaultSetting();
    }

    private void RefreshUI()
    {
        earnedMoneyText.text = GameLevelManager.Instance.EarnedCoin.ToString();
        currentDayText.text = "Day " + GameDataManager.Instance.CurrentDay.ToString();
    }

    private void SetDefaultSetting()
    {
        ShowCustomerRoom();
    }

    private void ShowCustomerRoom()
    {
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(true);

        playerEventSO.Event.OnAnyUIClosed?.Invoke();
        customerRoomUI.Show();
    }

    private void ShowAlchemyRoom()
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(false);

        playerEventSO.Event.OnAnyUIClosed?.Invoke();
        playerEventSO.Event.OnAlchemyRoomOpened?.Invoke();
    }

    private void ShowRecipeBook()
    {
        playerEventSO.Event.OnRecipeBookOpened?.Invoke();
    }
}
