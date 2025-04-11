using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopCraftingRoomUI : BaseUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnCraftingRoomOpened += HandleAlchemyRoomOpened;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnCraftingRoomOpened -= HandleAlchemyRoomOpened;
        }
    }

    /// <summary>
    /// Handles the event when the Alchemy Room is opened.
    /// </summary>
    private void HandleAlchemyRoomOpened()
    {
        Show();
    }
}
