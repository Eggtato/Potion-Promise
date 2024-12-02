using Eggtato.Utility;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField] private PlayerEventSO playerEventSO;

    [Header("Buttons")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button recipeBook;

    public new void Awake()
    {
        base.Awake();

        leftButton.onClick.AddListener(HandleCustomerRoomOpened);
        rightButton.onClick.AddListener(HandleAlchemyRoomOpened);
        recipeBook.onClick.AddListener(HandleRecipeBookOpened);
    }

    private void Start()
    {
        SetDefaultSetting();
    }

    private void SetDefaultSetting()
    {
        HandleCustomerRoomOpened();
    }

    private void HandleCustomerRoomOpened()
    {
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(true);

        playerEventSO.Event.OnAnyUIClosed?.Invoke();
        playerEventSO.Event.OnCustomerRoomOpened?.Invoke();
    }

    private void HandleAlchemyRoomOpened()
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(false);

        playerEventSO.Event.OnAnyUIClosed?.Invoke();
        playerEventSO.Event.OnAlchemyRoomOpened?.Invoke();
    }

    private void HandleRecipeBookOpened()
    {
        playerEventSO.Event.OnRecipeBookOpened?.Invoke();
    }
}
