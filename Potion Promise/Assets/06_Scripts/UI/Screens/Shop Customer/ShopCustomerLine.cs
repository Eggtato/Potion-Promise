using UnityEngine;

public class ShopCustomerLine : MonoBehaviour
{
    private ShopCustomerImageUI currentCustomer;

    public bool IsOccupied => currentCustomer != null;
    public ShopCustomerImageUI CurrentCustomer => currentCustomer;


    public void AssignCustomer(ShopCustomerImageUI customerUI)
    {
        currentCustomer = customerUI;
        customerUI.transform.SetParent(transform, false);
        customerUI.transform.localPosition = Vector3.zero;
    }

    public void Clear()
    {
        currentCustomer = null;
    }

    public ShopCustomerImageUI GetCurrentCustomer()
    {
        return currentCustomer;
    }
}
