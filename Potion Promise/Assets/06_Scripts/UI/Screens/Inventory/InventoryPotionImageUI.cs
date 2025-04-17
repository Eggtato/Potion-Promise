using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class InventoryPotionImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PotionData PotionData { get; private set; }

    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private Image icon;
    private Transform rootcanvasParent;

    private Transform parentAfterDrag;
    private bool isInteractable;

    private void Awake()
    {
        // icon = GetComponent<Image>();
        rootcanvasParent = GetComponentInParent<Canvas>().transform;
    }

    public void Initialize(PotionData potionData, bool isInteractable)
    {
        PotionData = potionData;
        icon.sprite = potionData?.Sprite;
        this.isInteractable = isInteractable;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        AudioManager.Instance.PlayMaterialGrabSound();

        transform.localScale = Vector2.zero;
        transform.DOScale(Vector3.one, 0.1f);

        parentAfterDrag = transform.parent;
        transform.SetParent(rootcanvasParent);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;

        GameLevelManager.Instance.RemoveCraftedPotionByOne(PotionData.PotionType);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.DOMove(new Vector3(mousePosition.x, mousePosition.y, 0), 0.1f);

        playerEventSO.Event.OnCursorSetGrab?.Invoke();
        playerEventSO.Event.OnDraggingInventoryMaterial?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        AudioManager.Instance.PlayMaterialReleaseSound();

        TryDropToDroppableArea();

        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
        icon.raycastTarget = true;

        playerEventSO.Event.OnCursorSetDefault?.Invoke();
        playerEventSO.Event.OnReleasedInventoryMaterial?.Invoke();
    }

    private void TryDropToDroppableArea()
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
                dropArea.ReceivePotion(PotionData.PotionType);
                transform.DOKill();
                Destroy(gameObject); // or pool it
                return;
            }
            else if (result.gameObject.TryGetComponent<CustomerPotionDropUI>(out var customerPotionDrop))
            {
                transform.DOKill();
                Destroy(gameObject); // or pool it
                return;
            }
        }

        // If it fails
        GameLevelManager.Instance.AddCraftedPotion(PotionData.PotionType);
        transform.DOKill();
        Destroy(gameObject); // or pool it
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable) return;

        playerEventSO.Event.OnCursorSetHand?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable) return;

        playerEventSO.Event.OnCursorSetDefault?.Invoke();
    }

}
