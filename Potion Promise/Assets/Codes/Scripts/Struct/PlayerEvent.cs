using System;
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
    public Action<MaterialData> OnMaterialCrafted;
}
