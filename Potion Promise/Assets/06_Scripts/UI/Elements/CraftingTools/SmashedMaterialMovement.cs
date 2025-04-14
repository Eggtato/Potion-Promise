using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmashedMaterialMovement : MonoBehaviour, IGrabbable
{
    [Header("References")]
    [SerializeField] private GameSettingSO gameSettingSO;
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sorting Order")]
    [SerializeField] private int sortingOrderOnGrab = 22;

    private Rigidbody2D myRigidbody2D;

    private MaterialData materialData;
    private Vector3 offset;
    private float zDistanceToCamera;

    private int originalSortingOrder;
    private bool isDragging = false;

    public MaterialData MaterialData => materialData;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
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
            DragObject();
        }
    }

    public void Initialize(MaterialData data)
    {
        materialData = data;
        spriteRenderer.color = data.Color;
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
        AudioManager.Instance.PlayMaterialGrabSound();

        offset = transform.position - GetWorldMousePosition();
        isDragging = true;
        myRigidbody2D.simulated = false;

        spriteRenderer.sortingOrder = sortingOrderOnGrab;
        EnableOutline();
    }

    private void StopDragging()
    {
        AudioManager.Instance.PlayMaterialReleaseSound();

        isDragging = false;
        myRigidbody2D.simulated = true;

        TryDropToTrashBin();
        DisableOutline();
    }

    private void DragObject()
    {
        Vector3 targetPosition = GetWorldMousePosition() + offset;
        transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, 10), 0.1f);
    }

    private void TryDropToTrashBin()
    {
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent<TrashBinUI>(out var trashBin))
            {
                transform.DOScale(0f, gameSettingSO.CraftingMaterialFadeInAnimation)
                         .OnComplete(() =>
                         {
                             AudioManager.Instance.PlayTrashBinSound();
                             Destroy(gameObject); // Pooling can be used instead
                         });
                return;
            }
        }

        spriteRenderer.sortingOrder = originalSortingOrder;
    }

    private Vector3 GetWorldMousePosition()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(screenPosition);
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
