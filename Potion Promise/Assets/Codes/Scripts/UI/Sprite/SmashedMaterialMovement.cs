using UnityEngine;

public class SmashedMaterialMovement : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float fixedXPosition = 0f; // Fixed X position to lock horizontal movement

    [SerializeField] private MortarHandler mortarHandler;

    private Vector3 offset;
    private bool isDragging = false;
    private float zDistanceToCamera;
    private bool directionChanged = false;
    private bool movedDown = false;
    private Rigidbody2D myRigidbody2D;
    private MaterialData materialData;
    private SpriteRenderer spriteRenderer;

    public MaterialData MaterialData => materialData;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Cache the initial Z distance from the camera
        zDistanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    private void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }

    public void Initialize(MaterialData materialData)
    {
        this.materialData = materialData;
        spriteRenderer.color = materialData.Color;
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        StopDragging();
    }

    /// <summary>
    /// Initiates dragging by calculating the offset.
    /// </summary>
    private void StartDragging()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();
        offset = transform.position - worldMousePosition;
        isDragging = true;

        myRigidbody2D.simulated = false;
    }

    /// <summary>
    /// Stops dragging.
    /// </summary>
    private void StopDragging()
    {
        isDragging = false;
        myRigidbody2D.simulated = true;
    }

    /// <summary>
    /// Handles the dragging behavior, clamping movement to specified boundaries.
    /// </summary>
    private void DragObject()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();
        Vector3 desiredPosition = worldMousePosition + offset;

        // Smoothly move the object to the desired position
        transform.position = desiredPosition;
    }

    /// <summary>
    /// Gets the mouse position in world space.
    /// </summary>
    /// <returns>Mouse position in world coordinates.</returns>
    private Vector3 GetWorldMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
