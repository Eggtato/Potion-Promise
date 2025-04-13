using Eggtato.Utility;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [Header("Pages")]
    [SerializeField] private ShopCustomerRoomUI customerRoomUI;
    [SerializeField] private ShopCraftingRoomUI craftingRoomUI;

    [Header("Texts")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text earnedCoinText;
    [SerializeField] private TMP_Text currentDayText;

    [Header("Buttons")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button recipeBook;
    [SerializeField] private Button dayButton;

    [Header("Feedbacks")]
    [SerializeField] private MMFeedbacks earnCoinFeedbacks;
    [SerializeField] private MMFeedbacks dayClickFeedbacks;

    private ShopInventoryUI shopInventoryUI;

    public new void Awake()
    {
        base.Awake();

        shopInventoryUI = GetComponent<ShopInventoryUI>();

        leftButton.onClick.AddListener(ShowCustomerRoom);
        rightButton.onClick.AddListener(ShowCraftingRoom);
        recipeBook.onClick.AddListener(ShowRecipeBook);
        dayButton.onClick.AddListener(HandleDayButtonClick);
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnEarnedCoinChanged += RefreshUI;
        playerEventSO.Event.OnCoinEarned += RefreshEarnedCoinUI;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnEarnedCoinChanged -= RefreshUI;
        playerEventSO.Event.OnCoinEarned -= RefreshEarnedCoinUI;
    }

    private void Start()
    {
        RefreshUI();
        SetDefaultSetting();
    }

    private void RefreshUI()
    {
        coinText.text = GameLevelManager.Instance.EarnedCoin.ToString();
        currentDayText.text = GameDataManager.Instance.CurrentDay.ToString();
    }

    private void RefreshEarnedCoinUI(int amount)
    {
        AudioManager.Instance.PlayCoinSound(SoundLength.Long);

        earnedCoinText.text = "+" + amount.ToString();
        earnCoinFeedbacks.PlayFeedbacks();
    }

    private void SetDefaultSetting()
    {
        ShowCustomerRoom();
    }

    private void ShowCustomerRoom()
    {
        AudioManager.Instance.PlayClickSound();

        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(true);

        playerEventSO.Event.OnAnyUIClosed?.Invoke();
        customerRoomUI.Show();
        shopInventoryUI.ShowPotionPanel();
    }

    private void ShowCraftingRoom()
    {
        AudioManager.Instance.PlayClickSound();

        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(false);

        playerEventSO.Event.OnAnyUIClosed?.Invoke();
        playerEventSO.Event.OnCraftingRoomOpened?.Invoke();
        craftingRoomUI.Show();
        shopInventoryUI.ShowMaterialPanel();
    }

    private void ShowRecipeBook()
    {
        AudioManager.Instance.PlayClickSound();

        playerEventSO.Event.OnRecipeBookOpened?.Invoke();
    }

    private void HandleDayButtonClick()
    {
        AudioManager.Instance.PlayClickSound();

        dayClickFeedbacks.PlayFeedbacks();
    }
}
