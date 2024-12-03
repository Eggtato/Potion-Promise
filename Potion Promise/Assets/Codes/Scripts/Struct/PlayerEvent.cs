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
    public Action OnCustomerRoomOpened;
    public Action OnAlchemyRoomOpened;
    public Action OnRecipeBookOpened;
    public Action OnMaterialSmashed;
    public Action OnMaterialStirred;
    public Action OnMaterialGetInCauldron;

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
