using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MaterialData MaterialData { get; private set; }

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
        AudioManager.Instance.PlayTypeSound();
        parentAfterDrag = transform.parent;
        transform.SetParent(rootCanvasParent);
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
                AudioManager.Instance.PlayTypeSound();
                mortarHandler.SetDroppedMaterial(MaterialData);
                GameLevelManager.Instance.RemoveObtainedMaterialByOne(MaterialData);
                return;
            }
        }
        AudioManager.Instance.PlayTypeSound();
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
