using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmashedMaterialMovement : MonoBehaviour, IGrabbable
{
    [Header("Project Reference")]
    [SerializeField] private GameSettingSO gameSettingSO;
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int grabSortingOrder = 22;

    private Vector3 offset;
    private bool isDragging = false;
    private float zDistanceToCamera;
    private Rigidbody2D myRigidbody2D;
    private MaterialData materialData;
    private int initialSortingOrder;

    public MaterialData MaterialData => materialData;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Cache the initial Z distance from the camera
        zDistanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        initialSortingOrder = spriteRenderer.sortingOrder;
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

    public void OnGrab()
    {
        StartDragging();
    }

    public void OnRelease()
    {
        StopDragging();
    }

    /// <summary>
    /// Initiates dragging by calculating the offset.
    /// </summary>
    private void StartDragging()
    {
        AudioManager.Instance.PlayMaterialGrabSound();

        Vector3 worldMousePosition = GetWorldMousePosition();
        offset = transform.position - worldMousePosition;
        isDragging = true;

        myRigidbody2D.simulated = false;

        spriteRenderer.sortingOrder = grabSortingOrder;
    }

    /// <summary>
    /// Stops dragging.
    /// </summary>
    private void StopDragging()
    {
        AudioManager.Instance.PlayMaterialReleaseSound();

        isDragging = false;
        myRigidbody2D.simulated = true;

        TryDropToTrashBin();
    }

    /// <summary>
    /// Handles the dragging behavior, clamping movement to specified boundaries.
    /// </summary>
    private void DragObject()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();
        Vector3 desiredPosition = worldMousePosition + offset;

        // Smoothly move the object to the desired position
        transform.DOMove(new Vector3(desiredPosition.x, desiredPosition.y, 10), 0.1f);
    }

    private void TryDropToTrashBin()
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
            if (result.gameObject.TryGetComponent<TrashBinUI>(out var trashArea))
            {
                transform.DOScale(0, gameSettingSO.CraftingMaterialFadeInAnimation).OnComplete(() =>
                {
                    AudioManager.Instance.PlayTrashBinSound();

                    spriteRenderer.sortingOrder = grabSortingOrder;
                    transform.DOKill();
                    Destroy(gameObject); // or pool it
                    return;
                });
            }
        }
        spriteRenderer.sortingOrder = initialSortingOrder;
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
