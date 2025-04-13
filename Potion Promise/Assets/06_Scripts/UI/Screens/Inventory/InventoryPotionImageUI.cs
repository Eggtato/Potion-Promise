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
                dropArea.ReceivePotion(PotionData.PotionType);
                transform.DOKill();
                Destroy(gameObject); // or pool it
                return;
            }
        }
        GameLevelManager.Instance.AddCraftedPotion(PotionData.PotionType);
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
