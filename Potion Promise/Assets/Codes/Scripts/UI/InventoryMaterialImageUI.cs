using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MaterialData MaterialData { get; private set; }

    private Transform rootcanvasParent;

    private Image icon;
    private Transform parentAfterDrag;

    private void Awake()
    {
        rootcanvasParent = GetComponentInParent<Canvas>().transform;
        icon = GetComponent<Image>();
    }

    public void Initialize(MaterialData materialData)
    {
        MaterialData = materialData;
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

        // Convert mouse position to world point
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Perform a 2D raycast to detect a collider
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null)
        {
            // Check if the object has the MortarDropAreaUI component
            var mortarHandler = hit.collider.GetComponent<MortarHandler>();
            if (mortarHandler != null)
            {
                mortarHandler.SetDroppedMaterial(MaterialData);
            }
        }
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
