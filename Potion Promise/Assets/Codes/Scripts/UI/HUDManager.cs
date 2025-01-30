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

        leftButton.onClick.AddListener(ShowCustomerRoom);
        rightButton.onClick.AddListener(ShowAlchemyRoom);
        recipeBook.onClick.AddListener(ShowRecipeBook);
    }

    private void Start()
    {
        SetDefaultSetting();
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
        playerEventSO.Event.OnCustomerRoomOpened?.Invoke();
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
