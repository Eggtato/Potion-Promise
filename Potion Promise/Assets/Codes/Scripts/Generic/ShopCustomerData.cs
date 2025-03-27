using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ShopCustomerData
{
    [PreviewField(60), HideLabel] public Sprite Sprite;
    public List<ShopCustomerOrderData> Orders = new List<ShopCustomerOrderData>();

    public ShopCustomerOrderData GetRandomOrder()
    {
        if (Orders == null || Orders.Count == 0)
            return null;

        return Orders[Random.Range(0, Orders.Count)];
    }
}
