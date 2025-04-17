using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialDropAreaUI : MonoBehaviour, IDropHandler
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [SerializeField] private DroppedMaterialMovement droppedMaterialPrefab;
    [SerializeField] private Transform parent;

    private bool isVisible;

    private void Start()
    {
        // gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnCraftingRoomOpened += HandleCraftingRoomOpened;
        playerEventSO.Event.OnCustomerRoomOpened += HandleCraftingRoomClosed;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnCraftingRoomOpened -= HandleCraftingRoomOpened;
        playerEventSO.Event.OnCustomerRoomOpened -= HandleCraftingRoomClosed;
    }

    private void HandleCraftingRoomOpened()
    {
        isVisible = true;
    }

    private void HandleCraftingRoomClosed()
    {
        isVisible = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!isVisible) return;

        var droppedMaterial = eventData.pointerDrag.GetComponent<InventoryMaterialImageUI>();

        if (droppedMaterial == null) return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // GameLevelManager.Instance.RemoveObtainedMaterialByOne(droppedMaterial.MaterialData);
        DroppedMaterialMovement material = Instantiate(droppedMaterialPrefab, worldPoint, Quaternion.identity);
        material.Initialize(droppedMaterial.MaterialData);
        material.transform.parent = parent;
    }
}
