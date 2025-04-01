using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopCustomerDatabaseSO", menuName = "Eggtato/Shop Customer Database")]
public class ShopCustomerDatabaseSO : ScriptableObject
{
    public List<ShopCustomerData> ShopCustomerDatas;

    public ShopCustomerData GetRandomCustomer() =>
        ShopCustomerDatas[Random.Range(0, ShopCustomerDatas.Count)];
}

