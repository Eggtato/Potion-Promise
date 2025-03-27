using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerPotionDropUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<InventoryPotionImageUI>(out var draggableItem))
        {
            Debug.Log("Potion dropped successfully!");
        }
        else
        {
            Debug.LogError("Dragged object does NOT have InventoryPotionImageUI component.");
        }
    }
}
