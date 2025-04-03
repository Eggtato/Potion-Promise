using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryMaterialImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public MaterialData MaterialData { get; private set; }

    private Transform rootcanvasParent;

    [SerializeField] private Image icon;
    private Transform parentAfterDrag;

    [SerializeField] private Image slotCardImage;

    public Sprite selectedSlotCardSprite;
    public Sprite unselectedSlotCardSprite;

    public bool inGathering = false;
    public bool selected = false;

    private RewardManager rewardManager;


    private void Awake()
    {
        rootcanvasParent = GetComponentInParent<Canvas>().transform;
    }

    public void Initialize(MaterialData materialData)
    {
        MaterialData = materialData;
        icon.sprite = materialData.Sprite;

    }

    public void InitializeInGathering(MaterialData materialData, RewardManager rewardManager, GameAssetSO gameAssetSO)
    {
        this.MaterialData = materialData;
        icon.sprite = materialData.Sprite;
        this.rewardManager = rewardManager;

        switch ((int)materialData.Rarity)
        {
            case 0:
                this.unselectedSlotCardSprite = gameAssetSO.MaterialCommmonCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedMaterialCommmonCard;
                break;
            case 1:
                this.unselectedSlotCardSprite = gameAssetSO.MaterialRareCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedMaterialRareCard;
                break;
            case 2:
                this.unselectedSlotCardSprite = gameAssetSO.MaterialEpicCard;
                this.selectedSlotCardSprite = gameAssetSO.SelectedMaterialEpicCard;
                break;

        }

        slotCardImage.sprite = unselectedSlotCardSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inGathering) return;

        AudioManager.Instance.PlayTypeSound();
        parentAfterDrag = transform.parent;
        transform.SetParent(rootcanvasParent);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inGathering) return;

        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inGathering) return;

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

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!inGathering) return;

        if (selected)
        {
            rewardManager.UnselectAll();
        }
        else
        {

            slotCardImage.sprite = selectedSlotCardSprite;

            rewardManager.SetMaterialSelected(MaterialData);

            selected = true;
        }
    }

    public void Unselect()
    {
        selected = false;

        slotCardImage.sprite = unselectedSlotCardSprite;
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
