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
            GameDataManager.Instance.RemoveCraftedPotionByOne(droppedPotion.PotionData.PotionType);
        }
        else
        {
            Debug.LogError("Dragged object does NOT have InventoryPotionImageUI component.");
        }
    }
}
