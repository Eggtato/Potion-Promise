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

    // Game Data Event
    public Action OnCurrentDayChanged;
    public Action OnDayEnd;
    public Action OnEarnedCoinChanged;
    public Action OnMaterialInventoryChanged;
    public Action OnPotionInventoryChanged;

    // UI Input Events : Main Menu
    public Action OnGoToNextScene;
    public Action OnNewGameButtonClicked;
    public Action OnContinueGameButtonClicked;
    public Action OnSettingButtonClicked;
    public Action OnExitButtonClicked;

    // UI Input Events : Main Menu > Setting
    public Action<SettingTabUI> OnSettingTabButtonClicked;

    // UI Input Events : Customer
    public Action<PotionData> OnPotionDroppedOnCustomer;

    // UI Input Events : Shop
    public Action OnCustomerRoomOpened;
    public Action OnAlchemyRoomOpened;
    public Action OnRecipeBookOpened;
    public Action OnMaterialSmashed;
    public Action OnMaterialStirred;

    // UI Input Events : Shop > Book Recipe
    public Action OnOpenShopButtonClicked;

    // UI Input Events : Shop > Book Recipe
    public Action<PotionDatabaseSO, MaterialDatabaseSO, GameAssetSO> OnRecipeBookPageInitialized;
    public Action OnAnyPageUIClosed;
    public Action OnPotionPageTabButtonClicked;
    public Action OnMaterialTabButtonClicked;
    public Action<PotionData> OnPotionSlotClicked;
    public Action<MaterialData> OnMaterialSlotClicked;

    // UI Input Events : Shop > Crafting
    public Action<MaterialData> OnMaterialGetInCauldron;
    public Action OnCauldronStirred;
    public Action<Vector3> OnSmashedMaterialDragging;
}
