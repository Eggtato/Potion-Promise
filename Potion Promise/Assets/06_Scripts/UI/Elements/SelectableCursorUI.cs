using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableCursorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerMoveHandler
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button == null || !button.IsInteractable()) return;

        playerEventSO.Event.OnCursorSetSelect?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button == null || !button.IsInteractable()) return;

        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button == null || !button.IsInteractable()) return;

        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (button == null || !button.IsInteractable()) return;

        playerEventSO.Event.OnCursorSetSelect?.Invoke();
    }
}
