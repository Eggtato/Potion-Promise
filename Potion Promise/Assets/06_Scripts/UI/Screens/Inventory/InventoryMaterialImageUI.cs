using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

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

        transform.localScale = Vector2.zero;
        transform.DOScale(Vector3.one, 0.1f);

        parentAfterDrag = transform.parent;
        transform.SetParent(rootCanvasParent);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;

        GameLevelManager.Instance.RemoveObtainedMaterialByOne(MaterialData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.DOMove(new Vector3(mousePosition.x, mousePosition.y, 0), 0.1f);

        playerEventSO.Event.OnCursorSetGrab?.Invoke();
        playerEventSO.Event.OnDraggingInventoryMaterial?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayMaterialReleaseSound();

        TryDropToInventory();

        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
        icon.raycastTarget = true;

        playerEventSO.Event.OnCursorSetDefault?.Invoke();
        playerEventSO.Event.OnReleasedInventoryMaterial?.Invoke();
    }

    private void TryDropToInventory()
    {
        // Perform a graphic raycast to check if the UI is under the cursor
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent<InventoryDropAreaUI>(out var dropArea))
            {
                dropArea.ReceiveMaterial(MaterialData);
                transform.DOKill();
                Destroy(gameObject); // or pool it
                return;
            }
        }

        // If it fails
        GameLevelManager.Instance.AddObtainedMaterial(MaterialData);
        transform.DOKill();
        Destroy(gameObject); // or pool it
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
