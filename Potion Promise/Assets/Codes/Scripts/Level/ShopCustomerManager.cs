using UnityEngine;

public class ShopCustomerManager : MonoBehaviour
{
    [SerializeField] private ShopCustomerDatabaseSO shopCustomerDatabaseSO;
    [SerializeField] private CustomerRoomUI customerRoomUI;

    private void Start()
    {
        GenerateRandomOrder();
    }

    public void GenerateRandomOrder()
    {
        ShopCustomerData customerData = shopCustomerDatabaseSO.ShopCustomerDatas[Random.Range(0, shopCustomerDatabaseSO.ShopCustomerDatas.Count)];
        ShopCustomerOrderData orderData = customerData.ShopCustomerOrderDatas[Random.Range(0, customerData.ShopCustomerOrderDatas.Count)];
        customerRoomUI.InitializeNPC(customerData.Sprite, orderData, () => GenerateRandomOrder());
    }
}
