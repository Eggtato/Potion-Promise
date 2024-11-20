using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private MaterialData assignedInventoryMaterial;
    [SerializeField] private Transform rootcanvasParent;
    [SerializeField] private TMP_Text quantitytxt;

    private Vector3 originalPosition;
    private Transform parentAfterDrag;

    void Start()
    {
        originalPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if(cursorcontroller.Instance != null)
    //         cursorcontroller.Instance.mouseGrabStartUI();
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if(cursorcontroller.Instance != null)
    //         cursorcontroller.Instance.defaultCursor();
    // }

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
        // if(transform.parent.parent.name == "CRAFTINGGRID"){
        //     if(eventData.pointerCurrentRaycast.gameObject.name == "SLOT" && assignedInventoryMaterial != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<PoundInitiator>().pm.haveMaterial==false && eventData.pointerCurrentRaycast.gameObject.GetComponent<PoundInitiator>().stage == 1){
        //         eventData.pointerCurrentRaycast.gameObject.GetComponent<PoundInitiator>().GetMaterial(assignedInventoryMaterial.ID);
        //         ReduceSelf();
        //     }
        //     GetComponent<RectTransform>().anchoredPosition = originalPosition;
        // }
        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
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
