using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialDropAreaUI : MonoBehaviour, IDropHandler
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [SerializeField] private DroppedMaterialMovement droppedMaterialPrefab;
    [SerializeField] private Transform parent;

    public void OnDrop(PointerEventData eventData)
    {
        var droppedMaterial = eventData.pointerDrag.GetComponent<InventoryMaterialImageUI>();

        if (droppedMaterial == null) return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // GameLevelManager.Instance.RemoveObtainedMaterialByOne(droppedMaterial.MaterialData);
        DroppedMaterialMovement material = Instantiate(droppedMaterialPrefab, worldPoint, Quaternion.identity);
        material.Initialize(droppedMaterial.MaterialData);
        material.transform.parent = parent;
    }
}
