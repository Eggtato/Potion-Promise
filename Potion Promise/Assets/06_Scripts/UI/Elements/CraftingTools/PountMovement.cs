using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PountMovement : MonoBehaviour
{
    [Header("Project References")]
    [SerializeField] private PlayerEventSO playerEventSO;

    [Header("Drag Settings")]
    [SerializeField] private float minYPosition = 0f; // Minimum Y boundary
    [SerializeField] private float maxYPosition = 2f; // Maximum Y boundary
    [SerializeField] private float fixedXPosition = 0f; // Fixed X position to lock horizontal movement
    [SerializeField] private float fadeOffset = 0.4f; // Offset for fading calculations
    [SerializeField] private float fadeDuration = 0.1f; // Duration for fade effects

    private Vector3 offset;
    private bool isDragging = false;
    private float zDistanceToCamera;
    private bool directionChanged = false;
    private bool movedDown = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private float initialGravityScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Cache the Z distance from the camera
        zDistanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        initialGravityScale = rigidBody.gravityScale;
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnSmashedMaterialDragging += HandleSmashedMaterialDragging;
        playerEventSO.Event.OnMaterialGetInCauldron += ResetColor;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnSmashedMaterialDragging -= HandleSmashedMaterialDragging;
        playerEventSO.Event.OnMaterialGetInCauldron -= ResetColor;
    }

    private void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        StopDragging();
    }

    private void HandleSmashedMaterialDragging(Vector3 smashedMaterialPosition)
    {
        float distanceFactor = Vector3.Distance(transform.position, smashedMaterialPosition) / 5f; // Divisor could be adjusted via a constant or serialized field
        float targetAlpha = Mathf.Clamp01(distanceFactor - fadeOffset);
        spriteRenderer.DOFade(targetAlpha, 0); // Instant fade
    }

    private void ResetColor(MaterialData materialData)
    {
        spriteRenderer.DOFade(1f, fadeDuration);
    }

    /// <summary>
    /// Initiates dragging by calculating the offset.
    /// </summary>
    private void StartDragging()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();
        offset = transform.position - worldMousePosition;
        isDragging = true;
    }

    /// <summary>
    /// Stops dragging.
    /// </summary>
    private void StopDragging()
    {
        rigidBody.gravityScale = initialGravityScale;
        isDragging = false;
    }

    /// <summary>
    /// Handles the dragging behavior, clamping movement to specified boundaries.
    /// </summary>
    private void DragObject()
    {
        rigidBody.gravityScale = 0;

        Vector3 worldMousePosition = GetWorldMousePosition();
        Vector3 desiredPosition = worldMousePosition + offset;

        // Clamp the Y position and fix the X position
        desiredPosition.x = fixedXPosition; // Lock X position to the specified value
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minYPosition, maxYPosition);

        // Move the object to the desired position
        transform.position = desiredPosition;

        HandlePoundSmashMovement(desiredPosition.y);
    }

    private void HandlePoundSmashMovement(float currentYPosition)
    {
        if (currentYPosition <= minYPosition && !directionChanged)
        {
            directionChanged = true;

            if (movedDown)
            {
                playerEventSO.Event.OnMaterialSmashed?.Invoke();
                movedDown = false;
            }
        }
        else if (currentYPosition >= maxYPosition && directionChanged)
        {
            directionChanged = false;
            movedDown = true;
        }
    }

    /// <summary>
    /// Gets the mouse position in world space.
    /// </summary>
    /// <returns>Mouse position in world coordinates.</returns>
    private Vector3 GetWorldMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zDistanceToCamera; // Maintain the initial Z distance from the camera
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
