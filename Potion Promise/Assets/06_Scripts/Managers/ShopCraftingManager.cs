using System.Collections.Generic;
using System.Linq;
using Eggtato.Utility;
using UnityEngine;

public class ShopCraftingManager : Singleton<ShopCraftingManager>
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;

    [Header("Scene Reference")]
    [SerializeField] private ShopCraftingRoomUI shopCraftingRoomUI;
    [SerializeField] private CraftedPotionUI craftedPotionUI;

    public void HandlePotionCrafted(List<MaterialType> materialTypeList)
    {
        PotionData craftedPotion = FindMatchingPotion(materialTypeList);

        if (craftedPotion != null)
        {
            craftedPotionUI.DisplayPotionSuccess(craftedPotion);
            HandlePotionSuccesfullyCrafted(craftedPotion.PotionType);
        }
        else
        {
            craftedPotionUI.DisplayPotionFailure();
        }
    }

    private PotionData FindMatchingPotion(List<MaterialType> materialTypeList)
    {
        return potionDatabaseSO.PotionDataList.FirstOrDefault(potion =>
            potion.MaterialRecipes.Count == materialTypeList.Count &&
            !potion.MaterialRecipes.Except(materialTypeList).Any() &&
            !materialTypeList.Except(potion.MaterialRecipes).Any());
    }

    public void HandlePotionSuccesfullyCrafted(PotionType potionType)
    {
        GameLevelManager.Instance.AddCraftedPotion(potionType);
    }
}
