using UnityEngine;
using UnityEngine.UI;

public class DragIconUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image iconImage;

    private void Awake()
    {
        Hide();
    }

    public void Show(Sprite sprite)
    {
        iconImage.sprite = sprite;
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
