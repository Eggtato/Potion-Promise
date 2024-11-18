using UnityEngine;

public class CustomerRoomUI : BaseUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnCustomerRoomOpened += HandleCustomerRoomOpened;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnCustomerRoomOpened -= HandleCustomerRoomOpened;
    }

    private void HandleCustomerRoomOpened()
    {
        Show();
    }
}
