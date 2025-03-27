using UnityEngine;

public class ShopCustomerManager : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [Header("Scene Reference")]
    [SerializeField] private ShopCustomerDatabaseSO shopCustomerDatabaseSO;
    [SerializeField] private CustomerRoomUI customerRoomUI;

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

    public void ProcessPotionDrop(PotionType potionType)
    {
        if (currentOrder.OrderedPotion == potionType)
            customerRoomUI.HandleCorrectCustomerOrder();
        else
            customerRoomUI.HandleIncorrectCustomerOrder();
    }
}
