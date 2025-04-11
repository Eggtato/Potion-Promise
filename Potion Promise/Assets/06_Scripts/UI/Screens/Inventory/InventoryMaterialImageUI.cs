using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MaterialData MaterialData { get; private set; }

    [SerializeField] private DroppedMaterialMovement droppedMaterial;

    private Transform rootCanvasParent;

    [SerializeField] private Image icon;
    private Transform parentAfterDrag;
    private Image image;

    private void Awake()
    {
        rootCanvasParent = GetComponentInParent<Canvas>().transform;
        image = GetComponent<Image>();
    }

    public void Initialize(MaterialData materialData)
    {
        MaterialData = materialData;
        icon.sprite = materialData.Sprite;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayTypeSound();
        parentAfterDrag = transform.parent;
        transform.SetParent(rootCanvasParent);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayTypeSound();
        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
        image.raycastTarget = true;
    }
}
