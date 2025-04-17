using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopCraftingRoomUI : BaseUI
{
    [SerializeField] private Transform craftingRoomBackground;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnCraftingRoomOpened += HandleCraftingRoomOpened;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnCraftingRoomOpened -= HandleCraftingRoomOpened;
        }
    }

    /// <summary>
    /// Handles the event when the Alchemy Room is opened.
    /// </summary>
    private void HandleCraftingRoomOpened()
    {
        Show();
    }
}
