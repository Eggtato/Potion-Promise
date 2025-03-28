using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerPotionDropUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private ShopCustomerRoomUI customerRoomUI;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<InventoryPotionImageUI>(out var droppedPotion))
        {
            customerRoomUI.PlayerEventSO.Event.OnPotionDroppedOnCustomer?.Invoke(droppedPotion.PotionData);
        }
        else
        {
            Debug.LogError("Dragged object does NOT have InventoryPotionImageUI component.");
        }
    }
}
