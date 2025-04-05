using UnityEngine;

public class ResetPositionUI : MonoBehaviour
{
    void Start()
    {
        var rect = (RectTransform)transform;
        rect.anchoredPosition = Vector2.zero;
    }
}
