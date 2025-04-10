using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopCustomerLineManager : MonoBehaviour
{
    [SerializeField] private List<ShopCustomerLine> customerLines;
    [SerializeField] private Transform shopCustomerParent;
    [SerializeField] private ShopCustomerImageUI shopCustomerPrefab;

    public bool TryAssignCustomerToLine(ShopCustomerData customerData, ShopCustomerOrderData orderData, out ShopCustomerImageUI customerUI)
    {
        foreach (var line in customerLines)
        {
            if (!line.IsOccupied)
            {
                customerUI = Instantiate(shopCustomerPrefab, shopCustomerParent);
                // customerUI.Setup(customerData);
                // customerUI.SetLineParent(line);
                line.AssignCustomer(customerUI);
                return true;
            }
        }

        customerUI = null;
        return false;
    }

    public bool HasAvailableLine()
    {
        return customerLines.Any(line => !line.IsOccupied);
    }

    public ShopCustomerImageUI GetFrontMostCustomer()
    {
        return customerLines.FirstOrDefault(line => line.IsOccupied)?.CurrentCustomer;
    }

}
