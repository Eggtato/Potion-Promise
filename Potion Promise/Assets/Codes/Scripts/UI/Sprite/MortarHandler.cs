using UnityEngine;

public class MortarHandler : MonoBehaviour
{
    [SerializeField] private GameAssetSO gameAssetSO;
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

    public void SmashMaterial()
    {
        if (materialData != null && currentSmashedCount < materialData.SmashedTimes - 1)
        {
            currentSmashedCount++;
            UpdateSmashedSprite();
        }
    }

    private void UpdateSmashedSprite()
    {
        int totalSprites = gameAssetSO.SmashedMaterialSprites.Count;

        // Calculate the range of smashes for each sprite
        int rangePerSprite = Mathf.CeilToInt((float)materialData.SmashedTimes / totalSprites);

        // Determine which sprite to use based on current smashed count
        int spriteIndex = Mathf.Min(currentSmashedCount / rangePerSprite, totalSprites - 1);

        droppedMaterialSprite.sprite = gameAssetSO.SmashedMaterialSprites[spriteIndex];
    }
}
