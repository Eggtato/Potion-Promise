using Eggtato.Utility;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DragIconManager : Singleton<DragIconManager>
{
    [Header("Project Reference")]
    [SerializeField] private GameSettingSO gameSettingSO;

    [SerializeField] private Image dragIcon;
    private CanvasGroup canvasGroup;

    private new void Awake()
    {
        base.Awake();

        canvasGroup = dragIcon.GetComponent<CanvasGroup>();
        InstantHide();
    }

    private void Start()
    {
        dragIcon.raycastTarget = false;
    }

    public void ShowIcon(Sprite sprite)
    {
        dragIcon.sprite = sprite;
        dragIcon.enabled = true;
        canvasGroup.alpha = 1;
        dragIcon.transform.DOScale(1, 0);
    }

    public void UpdatePosition(Vector2 screenPosition)
    {
        dragIcon.rectTransform.position = screenPosition;
    }

    public void Hide()
    {
        dragIcon.transform.DOScale(0, gameSettingSO.CraftingMaterialFadeInAnimation).OnComplete(() =>
        {
            dragIcon.enabled = false;
            canvasGroup.alpha = 0;
        });
    }

    public void InstantHide()
    {
        dragIcon.transform.DOScale(0, 0).OnComplete(() =>
        {
            dragIcon.enabled = false;
            canvasGroup.alpha = 0;
        });
    }
}
