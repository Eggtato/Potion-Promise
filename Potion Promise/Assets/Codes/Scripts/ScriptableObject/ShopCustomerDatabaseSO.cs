using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopCustomerDatabaseSO", menuName = "Eggtato/Shop Customer Database")]
public class ShopCustomerDatabaseSO : ScriptableObject
{
    [TableList(ShowPaging = true)] public List<ShopCustomerData> ShopCustomerDatas = new List<ShopCustomerData>();
}
