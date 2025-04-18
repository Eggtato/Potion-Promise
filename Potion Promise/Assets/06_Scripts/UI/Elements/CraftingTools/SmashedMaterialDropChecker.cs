using UnityEngine;

public class SmashedMaterialDropChecker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cauldronTopPart;
    [SerializeField] private Canvas cauldronWorldCanvas;
    [SerializeField] private int targetOrder = 20;

    private int cauldronTopPartInitialOrder;
    private int cauldronWorldCanvasInitialOrder;

    private void Start()
    {
        cauldronTopPartInitialOrder = cauldronTopPart.sortingOrder;
        cauldronWorldCanvasInitialOrder = cauldronWorldCanvas.sortingOrder;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SmashedMaterialMovement materialMovement))
        {
            if (materialMovement.transform.position.y > cauldronTopPart.transform.position.y)
            {
                cauldronTopPart.sortingOrder = targetOrder;
                cauldronWorldCanvas.sortingOrder = targetOrder;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        ResetSortingOrder();
    }

    public void ResetSortingOrder()
    {
        cauldronTopPart.sortingOrder = cauldronTopPartInitialOrder;
        cauldronWorldCanvas.sortingOrder = cauldronWorldCanvasInitialOrder;
    }
}
