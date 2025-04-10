using System.Collections.Generic;
using UnityEngine;

public class ShopCustomerQueueManager : MonoBehaviour
{
    private Queue<(ShopCustomerData, ShopCustomerOrderData)> customerQueue = new();
    public ShopCustomerOrderData CurrentOrderData { get; private set; }

    public void EnqueueCustomer(ShopCustomerData customer, ShopCustomerOrderData order)
    {
        customerQueue.Enqueue((customer, order));
    }

    public bool HasCustomerInQueue() => customerQueue.Count > 0;

    public (ShopCustomerData, ShopCustomerOrderData) GetNextCustomer()
    {
        var next = customerQueue.Dequeue();
        CurrentOrderData = next.Item2;
        return next;
    }

    public void ClearQueue() => customerQueue.Clear();
}

