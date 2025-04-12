using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppedMaterialMovement : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private int orderWhenDragging = 20;

    private int orderWhenDropped;
    private Vector3 offset;
    private bool isDragging = false;
    private float zDistanceToCamera;
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
        orderWhenDropped = spriteRenderer.sortingOrder;
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
        spriteRenderer.sprite = materialData.Sprite;
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

        playerEventSO.Event.OnStartDraggingDroppedMaterial?.Invoke(this);

        spriteRenderer.sortingOrder = orderWhenDragging;

        DragIconManager.Instance.ShowIcon(materialData.Sprite);
    }

    private void StopDragging()
    {
        isDragging = false;
        myRigidbody2D.simulated = true;

        TryDropToInventory();

        playerEventSO.Event.OnReleasedDroppedMaterial?.Invoke();

        spriteRenderer.sortingOrder = orderWhenDropped;

        DragIconManager.Instance.Hide();

    }

    private void TryDropToInventory()
    {
        // Perform a graphic raycast to check if the UI is under the cursor
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent<InventoryDropAreaUI>(out var dropArea))
            {
                dropArea.ReceiveMaterial(this.materialData);
                transform.DOKill();
                Destroy(gameObject); // or pool it
                return;
            }
        }
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
        playerEventSO.Event.OnDraggingDroppedMaterial?.Invoke(this);
        DragIconManager.Instance.UpdatePosition(new Vector3(desiredPosition.x, desiredPosition.y, 0));

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            var dropArea = result.gameObject.GetComponent<InventoryDropAreaUI>();
            if (dropArea != null)
            {
                // dropArea.Show();
                DragIconManager.Instance.ShowIcon(MaterialData.Sprite);
                spriteRenderer.DOFade(0, 0);
                return;
            }
            else
            {
                DragIconManager.Instance.Hide();
                spriteRenderer.DOFade(1, 0);
            }
        }
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
