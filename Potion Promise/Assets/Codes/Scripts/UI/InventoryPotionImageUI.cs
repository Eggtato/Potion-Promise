using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryPotionImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PotionData PotionData { get; private set; }

    private Transform rootcanvasParent;

    private Image icon;
    private Transform parentAfterDrag;

    private void Awake()
    {
        icon = GetComponent<Image>();
        rootcanvasParent = GetComponentInParent<Canvas>().transform;
    }

    public void Initialize(PotionData potionData)
    {
        PotionData = potionData;
        icon.sprite = potionData.Sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(rootcanvasParent);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
    }

    void ReduceSelf()
    {
        // assignedInventoryMaterial.Quantity--;
        // if (assignedInventoryMaterial.Quantity <= 0)
        // {
        //     transform.parent.gameObject.SetActive(false);
        // }
        // quantitytxt.text = "x " + assignedInventoryMaterial.Quantity;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.raycastTarget = true;
        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();


    }

}
