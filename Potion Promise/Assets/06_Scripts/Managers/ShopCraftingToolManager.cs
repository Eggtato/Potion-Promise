using UnityEngine;
using DG.Tweening;

public class ShopCraftingToolManager : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private GameSettingSO gameSettingSO;

    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private Transform container;

    private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        HandleAlchemyRoomInstantClosed();
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnAnyUIClosed += HandleAlchemyRoomClosed;
        playerEventSO.Event.OnCraftingRoomOpened += HandleAlchemyRoomOpened;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnAnyUIClosed -= HandleAlchemyRoomClosed;
        playerEventSO.Event.OnCraftingRoomOpened -= HandleAlchemyRoomOpened;
    }

    private void HandleAlchemyRoomOpened()
    {
        container.gameObject.SetActive(true);

        foreach (var item in spriteRenderers)
        {
            item.DOFade(1, gameSettingSO.FadeInAnimation);
        }
    }

    private void HandleAlchemyRoomClosed()
    {
        foreach (var item in spriteRenderers)
        {
            item.DOFade(0, gameSettingSO.FadeInAnimation);
        }

        container.gameObject.SetActive(false);
    }

    private void HandleAlchemyRoomInstantClosed()
    {
        foreach (var item in spriteRenderers)
        {
            item.DOFade(0, 0);
        }

        container.gameObject.SetActive(false);
    }
}
