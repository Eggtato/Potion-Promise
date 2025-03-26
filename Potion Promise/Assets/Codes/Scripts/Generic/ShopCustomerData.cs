using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ShopCustomerData
{
    [PreviewField(60), HideLabel] public Sprite Sprite;
    public List<ShopCustomerOrderData> ShopCustomerOrderDatas = new List<ShopCustomerOrderData>();
}
