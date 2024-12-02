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
    public Action OnMaterialSmashed;
    public Action OnMaterialStirred;
    public Action OnMaterialGetInCauldron;
    public Action<MaterialData> OnMaterialCrafted;
    public Action<Vector3> OnSmashedMaterialDragging;
    public Action<List<MaterialType>> OnCraftPotionButtonClicked;
}
