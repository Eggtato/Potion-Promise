using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryPotionImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PotionData PotionData { get; private set; }

    [SerializeField] private Image icon;
    private Transform rootcanvasParent;

    private Transform parentAfterDrag;

    private void Awake()
    {
        // icon = GetComponent<Image>();
        rootcanvasParent = GetComponentInParent<Canvas>().transform;
    }

    public void Initialize(PotionData potionData)
    {
        PotionData = potionData;
        icon.sprite = potionData?.Sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayTypeSound();
        parentAfterDrag = transform.parent;
        transform.SetParent(rootcanvasParent);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.raycastTarget = true;
        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
        AudioManager.Instance.PlayTypeSound();

    }

}
