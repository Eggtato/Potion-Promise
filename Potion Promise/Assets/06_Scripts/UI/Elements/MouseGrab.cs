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
        IGrabbable closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Blocker"))
            {
                return;
            }

            var grabbable = hit.collider.GetComponent<IGrabbable>();
            if (grabbable != null)
            {
                float distance = Vector2.Distance(mouseWorldPos, hit.point);
                if (distance < closestDistance)
                {
                    closest = grabbable;
                    closestDistance = distance;
                }
            }
        }

        if (closest != null)
        {
            grabbedTarget = closest;
            grabbedTarget.OnGrab();
            isGrabbing = true;
            playerEventSO.Event.OnCursorSetGrab?.Invoke();
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
            // Hover has changed
            if (currentHovered != null)
            {
                playerEventSO.Event.OnCursorSetDefault?.Invoke();
                if (currentHovered != null) currentHovered.DisableOutline();
            }

            currentHovered = hovered;

            if (currentHovered != null)
            {
                playerEventSO.Event.OnCursorSetHand?.Invoke();
                if (currentHovered != null) currentHovered.EnableOutline();
            }
        }
        else if (currentHovered != null && !isGrabbing)
        {
            // If we're still hovering over the same object after releasing, set cursor to hand
            playerEventSO.Event.OnCursorSetHand?.Invoke();
        }
    }
}
