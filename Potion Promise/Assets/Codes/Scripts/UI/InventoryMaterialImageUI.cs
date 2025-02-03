using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MaterialData MaterialData { get; private set; }

    [SerializeField] private Transform rootcanvasParent;

    private Image icon;
    private Transform parentAfterDrag;

    public void Initialize(MaterialData materialData)
    {
        MaterialData = materialData;
        icon = GetComponent<Image>();
        icon.sprite = materialData.Sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(rootcanvasParent);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();

        var mortarDropAreaUI = eventData.pointerCurrentRaycast.gameObject.GetComponent<MortarDropAreaUI>();
        if (mortarDropAreaUI == null) return;

        mortarDropAreaUI.SetDroppedMaterial(MaterialData);
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
}
