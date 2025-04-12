using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableCursorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerMoveHandler
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        playerEventSO.Event.OnCursorSetSelect?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        playerEventSO.Event.OnCursorSetSelect?.Invoke();
    }
}
