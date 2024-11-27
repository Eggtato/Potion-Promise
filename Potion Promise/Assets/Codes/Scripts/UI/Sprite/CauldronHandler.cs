using UnityEngine;
using DG.Tweening;

public class CauldronHandler : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private float lastYDroppedSpritePosition = 2.3f;
    [SerializeField] private SpriteRenderer droppedMaterialSprite;

    private MaterialData materialData;
    private int currentSmashedCount = 0;
    private Vector3 initialDroppedSpritePosition;

    private void Start()
    {
        initialDroppedSpritePosition = droppedMaterialSprite.transform.localPosition;
        ResetCauldron();
    }

    public void SetDroppedMaterial(MaterialData materialData)
    {
        this.materialData = materialData;
        currentSmashedCount = 0;
        droppedMaterialSprite.color = materialData.Color;
        RaiseSmashedSprite();
    }

    private void ResetCauldron()
    {
        materialData = null;
        currentSmashedCount = 0;
    }

    public void StirMaterial()
    {
        if (materialData == null) return;

        if (materialData != null && currentSmashedCount < materialData.SmashedTimes - 1)
        {
            currentSmashedCount++;
        }
        else
        {
            // Assign to recipe list
            AddedCraftedMaterial();
            ResetCauldron();
            LowerSmashedSprite();
        }
    }

    private void RaiseSmashedSprite()
    {
        droppedMaterialSprite.transform.DOLocalMoveY(lastYDroppedSpritePosition, 0.5f);
    }

    private void LowerSmashedSprite()
    {
        droppedMaterialSprite.transform.DOLocalMoveY(initialDroppedSpritePosition.y, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<SmashedMaterialMovement>(out SmashedMaterialMovement material))
        {
            SetDroppedMaterial(material.MaterialData);
            Destroy(other.gameObject);
        }
    }

    public void AddedCraftedMaterial()
    {
        playerEventSO.Event.OnMaterialCrafted?.Invoke(materialData);
    }
}
