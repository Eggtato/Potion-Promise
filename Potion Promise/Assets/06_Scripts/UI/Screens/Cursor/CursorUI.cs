using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [System.Serializable]
    public class CursorReference
    {
        public CursorType CursorType;
        public Texture2D Texture;
        public Vector2 Hotspot = Vector2.zero;
    }

    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    public CursorReference[] cursors;

    private Dictionary<CursorType, CursorReference> cursorMap;

    private void Awake()
    {
        cursorMap = new Dictionary<CursorType, CursorReference>();
        foreach (var cursor in cursors)
        {
            cursorMap[cursor.CursorType] = cursor;
        }
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnCursorSetDefault += HandleDefault;
        playerEventSO.Event.OnCursorSetGrab += HandleGrab;
        playerEventSO.Event.OnCursorSetHand += HandleHand;
        playerEventSO.Event.OnCursorSetSelect += HandleSelect;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnCursorSetDefault -= HandleDefault;
        playerEventSO.Event.OnCursorSetGrab -= HandleGrab;
        playerEventSO.Event.OnCursorSetHand -= HandleHand;
        playerEventSO.Event.OnCursorSetSelect -= HandleSelect;
    }

    private void Start()
    {
        HandleDefault();
    }

    public void SetCursor(CursorType cursorType)
    {
        if (cursorMap.TryGetValue(cursorType, out CursorReference cursor))
        {
            Cursor.SetCursor(cursor.Texture, cursor.Hotspot, CursorMode.Auto);
        }
    }

    private void HandleGrab() => SetCursor(CursorType.Grab);
    private void HandleHand() => SetCursor(CursorType.Hand);
    private void HandleSelect() => SetCursor(CursorType.Select);

    public void HandleDefault()
    {
        SetCursor(CursorType.Default);
    }
}
