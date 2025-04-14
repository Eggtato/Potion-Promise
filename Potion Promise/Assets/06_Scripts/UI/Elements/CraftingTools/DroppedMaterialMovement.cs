using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppedMaterialMovement : MonoBehaviour, IGrabbable
{
    [Header("Settings")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private int orderWhenDragging = 20;

    private int originalSortingOrder;
    private Vector3 grabOffset;
    private float zDistanceToCamera;

    private bool isDragging = false;

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;

    private MaterialData materialData;
    public MaterialData MaterialData => materialData;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        zDistanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        originalSortingOrder = spriteRenderer.sortingOrder;
        DisableOutline();
    }

    private void Update()
    {
        if (isDragging)
        {
            UpdateDragging();
        }
    }

    public void Initialize(MaterialData materialData)
    {
        this.materialData = materialData;
        spriteRenderer.sprite = materialData.Sprite;
    }

    public void OnGrab()
    {
        StartDragging();
    }

    public void OnRelease()
    {
        StopDragging();
    }

    private void StartDragging()
    {
        isDragging = true;
        rb2D.simulated = false;

        grabOffset = transform.position - GetWorldMousePosition();
        spriteRenderer.sortingOrder = orderWhenDragging;

        AudioManager.Instance.PlayMaterialGrabSound();

        playerEventSO.Event.OnStartDraggingDroppedMaterial?.Invoke(this);
        playerEventSO.Event.OnCursorSetGrab?.Invoke();

        DragIconManager.Instance.ShowIcon(materialData.Sprite);
    }

    private void StopDragging()
    {
        isDragging = false;
        rb2D.simulated = true;
        spriteRenderer.sortingOrder = originalSortingOrder;

        TryDropToInventory();

        playerEventSO.Event.OnReleasedDroppedMaterial?.Invoke();
        playerEventSO.Event.OnCursorSetDefault?.Invoke();

        DragIconManager.Instance.Hide();
        spriteRenderer.material.DisableKeyword("OUTBASE_ON");
    }

    private void UpdateDragging()
    {
        Vector3 mousePosition = GetWorldMousePosition();
        Vector3 targetPosition = mousePosition + grabOffset;

        transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, 0), 0.1f);
        DragIconManager.Instance.UpdatePosition(targetPosition);
        playerEventSO.Event.OnDraggingDroppedMaterial?.Invoke(this);

        HandleUIDropFeedback();
    }

    private void HandleUIDropFeedback()
    {
        var results = RaycastUI(Input.mousePosition);
        bool overDropArea = false;

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<InventoryDropAreaUI>(out _))
            {
                overDropArea = true;
                break;
            }
        }

        if (overDropArea)
        {
            DragIconManager.Instance.ShowIcon(materialData.Sprite);
            spriteRenderer.DOFade(0, 0);
            DisableOutline();
        }
        else
        {
            DragIconManager.Instance.Hide();
            spriteRenderer.DOFade(1, 0);
            EnableOutline();
        }
    }

    private void TryDropToInventory()
    {
        var results = RaycastUI(Input.mousePosition);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<InventoryDropAreaUI>(out var dropArea))
            {
                dropArea.ReceiveMaterial(materialData);
                transform.DOKill();
                Destroy(gameObject); // Or return to pool
                return;
            }
        }
    }

    private List<RaycastResult> RaycastUI(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results;
    }

    private Vector3 GetWorldMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void EnableOutline()
    {
        spriteRenderer.material.EnableKeyword("OUTBASE_ON");
    }

    public void DisableOutline()
    {
        spriteRenderer.material.DisableKeyword("OUTBASE_ON");
    }
}
