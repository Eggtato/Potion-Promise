using Eggtato.Utility;
using UnityEngine;
using UnityEngine.UI;

public class DragIconManager : Singleton<DragIconManager>
{
    [SerializeField] private Image dragIcon;
    private CanvasGroup canvasGroup;

    private new void Awake()
    {
        base.Awake();

        canvasGroup = dragIcon.GetComponent<CanvasGroup>();
        Hide();
    }

    public void ShowIcon(Sprite sprite)
    {
        dragIcon.sprite = sprite;
        dragIcon.enabled = true;
        canvasGroup.alpha = 1;
    }

    public void UpdatePosition(Vector2 screenPosition)
    {
        dragIcon.rectTransform.position = screenPosition;
    }

    public void Hide()
    {
        dragIcon.enabled = false;
        canvasGroup.alpha = 0;
    }
}
