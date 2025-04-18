using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;

    private IGrabbable grabbedTarget;
    private IGrabbable currentHovered;
    private bool isGrabbing = false;

    void Update()
    {
        if (Camera.main == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos;

        if (Input.GetMouseButtonDown(0))
        {
            TryGrab(mouseWorldPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopGrab();
        }

        HandleHover(mouseWorldPos);
    }

    void TryGrab(Vector3 mouseWorldPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero);

        // Sort hits by sorting order (visual priority)
        System.Array.Sort(hits, (a, b) =>
        {
            var srA = a.collider.GetComponent<SpriteRenderer>();
            var srB = b.collider.GetComponent<SpriteRenderer>();

            int layerCompare = 0;
            if (srA != null && srB != null)
            {
                layerCompare = SortingLayer.GetLayerValueFromID(srB.sortingLayerID).CompareTo(SortingLayer.GetLayerValueFromID(srA.sortingLayerID));
                if (layerCompare == 0)
                    return srB.sortingOrder.CompareTo(srA.sortingOrder); // Higher sortingOrder means more in front
                else
                    return layerCompare;
            }

            return 0;
        });

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Blocker"))
            {
                return;
            }

            if (hit.collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
                grabbedTarget = grabbable;
                grabbedTarget.OnGrab();
                isGrabbing = true;
                playerEventSO.Event.OnCursorSetGrab?.Invoke();
                return;
            }
        }
    }


    void StopGrab()
    {
        if (grabbedTarget != null)
        {
            grabbedTarget.OnRelease();
            grabbedTarget = null;
            isGrabbing = false;
        }

        // After releasing the grab, check if we're still hovering over something
        HandleHover(Camera.main.ScreenToWorldPoint(Input.mousePosition));  // Recheck hover state when releasing
    }

    void HandleHover(Vector3 mouseWorldPos)
    {
        if (isGrabbing) return;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero);

        // Sort hits based on visual priority (sorting layer and order)
        System.Array.Sort(hits, (a, b) =>
        {
            var srA = a.collider.GetComponent<SpriteRenderer>();
            var srB = b.collider.GetComponent<SpriteRenderer>();

            int layerCompare = 0;
            if (srA != null && srB != null)
            {
                layerCompare = SortingLayer.GetLayerValueFromID(srB.sortingLayerID)
                    .CompareTo(SortingLayer.GetLayerValueFromID(srA.sortingLayerID));
                if (layerCompare == 0)
                    return srB.sortingOrder.CompareTo(srA.sortingOrder);
                else
                    return layerCompare;
            }
            return 0;
        });

        IGrabbable hovered = null;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Blocker"))
            {
                hovered = null;
                break;
            }

            var grabbable = hit.collider.GetComponent<IGrabbable>();
            if (grabbable != null)
            {
                hovered = grabbable;
                break;
            }
        }

        if (hovered != currentHovered)
        {
            if (currentHovered != null)
            {
                playerEventSO.Event.OnCursorSetDefault?.Invoke();
                currentHovered.DisableOutline();
            }

            currentHovered = hovered;

            if (currentHovered != null)
            {
                playerEventSO.Event.OnCursorSetHand?.Invoke();
                currentHovered.EnableOutline();
            }
        }
        else if (currentHovered != null && !isGrabbing)
        {
            playerEventSO.Event.OnCursorSetHand?.Invoke();
        }
    }

}
