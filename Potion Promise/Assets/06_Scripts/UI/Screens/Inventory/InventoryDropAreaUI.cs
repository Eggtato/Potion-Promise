using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDropAreaUI : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    private void Start()
    {
        Hide();
        canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnStartDraggingDroppedMaterial += HandleStartDraggingMaterialDragging;
        playerEventSO.Event.OnDraggingDroppedMaterial += HandleDraggingMaterialDragging;
        playerEventSO.Event.OnReleasedDroppedMaterial += HandleReleasedMaterialDragging;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnStartDraggingDroppedMaterial -= HandleStartDraggingMaterialDragging;
        playerEventSO.Event.OnDraggingDroppedMaterial -= HandleDraggingMaterialDragging;
        playerEventSO.Event.OnReleasedDroppedMaterial -= HandleReleasedMaterialDragging;
    }

    private void HandleStartDraggingMaterialDragging(DroppedMaterialMovement droppedMaterial)
    {
        canvasGroup.blocksRaycasts = true;
    }

    private void HandleDraggingMaterialDragging(DroppedMaterialMovement droppedMaterial)
    {
        // No longer needed to handle icon movement here
    }

    private void HandleReleasedMaterialDragging()
    {
        Hide();
        canvasGroup.blocksRaycasts = false;
    }

    public void ReceiveMaterial(MaterialData data)
    {
        GameLevelManager.Instance.AddObtainedMaterial(data);
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
