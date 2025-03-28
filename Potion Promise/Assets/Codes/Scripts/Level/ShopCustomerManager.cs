using UnityEngine;

public class ShopCustomerManager : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private ShopCustomerDatabaseSO shopCustomerDatabaseSO;

    [Header("Scene Reference")]
    [SerializeField] private ShopCustomerRoomUI shopCustomerRoomUI;

    private ShopCustomerOrderData currentOrder;

    private void OnEnable()
    {
        playerEventSO.Event.OnPotionDroppedOnCustomer += ProcessPotionDrop;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnPotionDroppedOnCustomer -= ProcessPotionDrop;
    }

    private void Start()
    {
        shopCustomerRoomUI.Initialize(this);

        GenerateRandomOrder();
    }

    public void GenerateRandomOrder()
    {
        var customerData = shopCustomerDatabaseSO.GetRandomCustomer();
        var orderData = customerData.GetRandomOrder();

        currentOrder = orderData;
        shopCustomerRoomUI.InitializeNPC(customerData.Sprite, orderData);
    }

    public void RejectOrder()
    {
        GenerateRandomOrder();
    }

    public void ProcessPotionDrop(PotionData potionData)
    {
        if (currentOrder.OrderedPotion == potionData.PotionType)
        {
            GameLevelManager.Instance.AddEarnedCoin(potionData.Price);
            shopCustomerRoomUI.HandleCorrectCustomerOrder();
        }
        else
        {
            shopCustomerRoomUI.HandleIncorrectCustomerOrder();
        }
    }
}
