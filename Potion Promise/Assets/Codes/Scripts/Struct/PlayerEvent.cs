using System;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerEvent
{
    // Player Input Events
    public Action<GameObject, Vector3> OnMouseDown;
    public Action<Vector3> OnMouseUp;

    // UI Input Events
    public Action OnAnyUIClosed;

    // UI Input Events : Main Menu
    public Action OnGoToNextScene;
    public Action OnNewGameButtonClicked;
    public Action OnContinueGameButtonClicked;
    public Action OnSettingButtonClicked;
    public Action OnExitButtonClicked;

    // UI Input Events : Main Menu > Setting
    public Action<SettingTabUI> OnSettingTabButtonClicked;

    // UI Input Events : Customer
    public Action<PotionType> OnPotionDroppedOnCustomer;

    // UI Input Events : Shop
    public Action OnCustomerRoomOpened;
    public Action OnAlchemyRoomOpened;
    public Action OnRecipeBookOpened;
    public Action OnMaterialSmashed;
    public Action OnMaterialStirred;
    public Action OnMaterialGetInCauldron;

    // UI Input Events : Shop > Book Recipe
    public Action<PotionDatabaseSO, MaterialDatabaseSO, GameAssetSO> OnRecipeBookPageInitialized;
    public Action OnAnyPageUIClosed;
    public Action OnPotionPageTabButtonClicked;
    public Action OnMaterialTabButtonClicked;
    public Action<PotionData> OnPotionSlotClicked;
    public Action<MaterialData> OnMaterialSlotClicked;

    public Action<MaterialData> OnMaterialCrafted;
    public Action<Vector3> OnSmashedMaterialDragging;
    public Action<List<MaterialType>> OnCraftPotionButtonClicked;
}
