using UnityEngine;

public class AlchemyRoomUI : BaseUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        playerEventSO.Event.OnAlchemyRoomOpnened += HandleAlchemyRoomOpened;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerEventSO.Event.OnAlchemyRoomOpnened -= HandleAlchemyRoomOpened;
    }

    private void HandleAlchemyRoomOpened()
    {
        Show();
    }
}
