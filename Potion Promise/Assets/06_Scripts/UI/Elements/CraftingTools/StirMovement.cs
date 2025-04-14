using DG.Tweening;
using UnityEngine;

public class StirMovement : MonoBehaviour, IGrabbable
{
    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [Header("Rotation Settings")]
    [SerializeField] private float sensitivity = 0.1f; // Sensitivity for rotation based on mouse movement
    [SerializeField] private float minZRotation = -15.6f; // Minimum allowed Z rotation
    [SerializeField] private float maxZRotation = 15.6f;  // Maximum allowed Z rotation

    [SerializeField] private CauldronHandler caulronHandler;

    private bool isDragging = false;
    private Vector3 initialMousePosition; // Mouse position at the start of dragging
    private float initialZRotation;      // Object's Z rotation at the start of dragging
    private bool directionChanged = false;
    private bool wentLeft = false;

    private void Update()
    {
        if (isDragging)
        {
            RotateWithMouse();
        }
    }

    private void Start()
    {
        HandleStirReleased();
    }

    /// <summary>
    /// Initiates dragging by storing the initial mouse position and normalized Z rotation.
    /// </summary>
    public void OnGrab()
    {
        AudioManager.Instance.PlayStirGrabSound();

        initialMousePosition = Input.mousePosition;
        initialZRotation = GetNormalizedZRotation(); // Get the current rotation in the -180 to 180 range
        isDragging = true;
    }

    /// <summary>
    /// Ends dragging.
    /// </summary>
    public void OnRelease()
    {
        AudioManager.Instance.PlayStirGrabSound();

        isDragging = false;

        HandleStirReleased();
    }

    /// <summary>
    /// Handles object rotation based on mouse movement.
    /// </summary>
    private void RotateWithMouse()
    {
        Vector3 currentMousePosition = Input.mousePosition;

        // Calculate the horizontal mouse movement delta
        float deltaX = currentMousePosition.x - initialMousePosition.x;

        // Calculate the desired Z rotation, clamped within specified bounds
        float targetZRotation = Mathf.Clamp(initialZRotation - (deltaX * sensitivity), minZRotation, maxZRotation);

        // Apply the rotation to the object
        // transform.rotation = Quaternion.Euler(0, 0, targetZRotation);
        transform.DORotate(new Vector3(0, 0, targetZRotation), 0.1f);

        CheckForDirectionChange(targetZRotation);
    }

    void CheckForDirectionChange(float currentZRotation)
    {
        if (currentZRotation >= maxZRotation && directionChanged)
        {
            directionChanged = false;

            if (wentLeft)
            {
                AudioManager.Instance.PlayMaterialStirredInCauldronSound();

                playerEventSO.Event.OnMaterialStirred?.Invoke();
                wentLeft = false;
            }
        }
        else if (currentZRotation <= minZRotation && !directionChanged)
        {
            directionChanged = true;
            wentLeft = true;
        }
    }

    private void HandleStirReleased()
    {
        float currentZ = transform.eulerAngles.z;
        float distanceToMinZRotation = Mathf.Abs(Mathf.DeltaAngle(currentZ, minZRotation));
        float distanceToMaxZRotation = Mathf.Abs(Mathf.DeltaAngle(currentZ, maxZRotation));

        if (distanceToMinZRotation > distanceToMaxZRotation)
        {
            transform.DORotate(new Vector3(0, 0, maxZRotation), 0.2f);
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, minZRotation), 0.2f);
        }
    }

    /// <summary>
    /// Gets the current Z rotation normalized to the range -180 to 180.
    /// </summary>
    /// <returns>Normalized Z rotation.</returns>
    private float GetNormalizedZRotation()
    {
        float zRotation = transform.eulerAngles.z;
        return zRotation > 180f ? zRotation - 360f : zRotation;
    }
}
