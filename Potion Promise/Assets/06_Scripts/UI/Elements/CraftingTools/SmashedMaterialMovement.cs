using DG.Tweening;
using UnityEngine;

public class SmashedMaterialMovement : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private float zDistanceToCamera;
    private Rigidbody2D myRigidbody2D;
    private MaterialData materialData;
    private SpriteRenderer spriteRenderer;
    private PlayerEventSO playerEventSO;

    public MaterialData MaterialData => materialData;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerEventSO = GetComponentInParent<MortarHandler>().PlayerEventSO;
    }

    private void Start()
    {
        // Cache the initial Z distance from the camera
        zDistanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    private void Update()
    {
        playerEventSO.Event.OnSmashedMaterialDragging?.Invoke(transform.position);

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
        transform.DOMove(new Vector3(desiredPosition.x, desiredPosition.y, 0), 0.1f);
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
