using UnityEngine;
using DG.Tweening;

public class AlchemyRoomManager : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private Transform container;
    [SerializeField] protected float fadeTransitionTime = 0.3f;

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
            item.DOFade(1, fadeTransitionTime);
        }
    }

    private void HandleAlchemyRoomClosed()
    {
        foreach (var item in spriteRenderers)
        {
            item.DOFade(0, fadeTransitionTime);
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
