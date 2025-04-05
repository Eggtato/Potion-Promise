using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ShopCustomerOrderData
{
    [VerticalGroup("Split")][Multiline(3)] public string OrderDescription;
    [VerticalGroup("Split")][Multiline(2)] public string DeclineDescription;
    [VerticalGroup("Split")][Multiline(2)] public string CorrectOrderDescription;
    [VerticalGroup("Split")][Multiline(2)] public string InCorrectOrderDescription;
    public PotionType OrderedPotion;
}
