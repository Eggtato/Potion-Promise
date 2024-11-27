using UnityEngine;
using DG.Tweening;

public class CauldronHandler : MonoBehaviour
{
    [SerializeField] private float lastYDroppedSpritePosition = 2.3f;
    [SerializeField] private SpriteRenderer droppedMaterialSprite;

    private MaterialData materialData;
    private int currentSmashedCount = 0;

    private void Start()
    {
        droppedMaterialSprite.gameObject.SetActive(false);
    }

    public void SetDroppedMaterial(MaterialData materialData)
    {
        this.materialData = materialData;
        currentSmashedCount = 0;
        droppedMaterialSprite.gameObject.SetActive(true);
        UpdateSmashedSprite();
        droppedMaterialSprite.color = materialData.Color;
    }

    public void StirMaterial()
    {
        if (materialData != null && currentSmashedCount < materialData.SmashedTimes - 1)
        {
            currentSmashedCount++;
        }
        else
        {
            // Assign to recipe list
            Debug.Log("Done");
        }
    }

    private void UpdateSmashedSprite()
    {
        droppedMaterialSprite.color = materialData.Color;
        droppedMaterialSprite.transform.DOLocalMoveY(lastYDroppedSpritePosition, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<SmashedMaterialMovement>(out SmashedMaterialMovement material))
        {
            SetDroppedMaterial(material.MaterialData);
            Destroy(other.gameObject);
        }
    }
}
