using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MMFeedbacks hoverFeedbacks;
    [SerializeField] private MMFeedbacks unHoverFeedbacks;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverFeedbacks.PlayFeedbacks();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        unHoverFeedbacks.PlayFeedbacks();
    }
}
