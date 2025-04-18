using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerPotionDropUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private PlayerEventSO playerEventSO;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<InventoryPotionImageUI>(out var droppedPotion))
        {
            playerEventSO.Event.OnPotionDroppedOnCustomer?.Invoke(droppedPotion.PotionData);
            GameLevelManager.Instance.RemoveCraftedPotionByOne(droppedPotion.PotionData.PotionType);
            GameLevelManager.Instance.AddSoldPotion(droppedPotion.PotionData.PotionType);
        }
        else
        {
            Debug.LogError("Dragged object does NOT have InventoryPotionImageUI component.");
        }
    }
}
