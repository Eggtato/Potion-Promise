using UnityEngine;
using UnityEngine.UI;

public class ShopCustomerImageUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup => canvasGroup;
    public ShopCustomerData ShopCustomerData { get; private set; }
    public ShopCustomerOrderData ShopCustomerOrderData { get; private set; }

    public void InitilizeCustomer(ShopCustomerData shopCustomerData, ShopCustomerOrderData shopCustomerOrderData)
    {
        ShopCustomerData = shopCustomerData;
        ShopCustomerOrderData = shopCustomerOrderData;

        image.sprite = shopCustomerData.Sprite;
    }
}
