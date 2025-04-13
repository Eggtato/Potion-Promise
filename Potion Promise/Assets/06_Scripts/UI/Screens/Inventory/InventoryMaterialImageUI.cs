using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public MaterialData MaterialData { get; private set; }


    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private DroppedMaterialMovement droppedMaterial;

    private Transform rootCanvasParent;

    [SerializeField] private Image icon;
    private Transform parentAfterDrag;

    private void Awake()
    {
        rootCanvasParent = GetComponentInParent<Canvas>().transform;
    }

    public void Initialize(MaterialData materialData)
    {
        MaterialData = materialData;
        icon.sprite = materialData.Sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayMaterialGrabSound();

        parentAfterDrag = transform.parent;
        transform.SetParent(rootCanvasParent);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        playerEventSO.Event.OnCursorSetGrab?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayMaterialReleaseSound();

        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
        icon.raycastTarget = true;

        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        playerEventSO.Event.OnCursorSetHand?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

}
