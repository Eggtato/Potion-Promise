using UnityEngine;

public class ShopCustomerManager : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private ShopCustomerDatabaseSO shopCustomerDatabaseSO;

    [Header("Scene Reference")]
    [SerializeField] private ShopCustomerRoomUI customerRoomUI;

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
        customerRoomUI.Initialize(this);

        GenerateRandomOrder();
    }

    public void GenerateRandomOrder()
    {
        var customerData = shopCustomerDatabaseSO.GetRandomCustomer();
        var orderData = customerData.GetRandomOrder();

        currentOrder = orderData;
        customerRoomUI.InitializeNPC(customerData.Sprite, orderData);
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
            customerRoomUI.HandleCorrectCustomerOrder();
        }
        else
        {
            customerRoomUI.HandleIncorrectCustomerOrder();
        }
    }
}
