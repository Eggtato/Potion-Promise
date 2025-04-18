using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CauldronHandler : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private PlayerEventSO playerEventSO; // Reference to player events

    [Header("Scene References")]
    [SerializeField] private SpriteRenderer droppedMaterialSprite; // Sprite representing the dropped material
    [SerializeField] private CraftingToolMaterialUI craftingToolMaterialUI;
    [SerializeField] private CraftingToolMaterialProgressUI craftingToolProgressUI;

    [Header("Configs")]
    [SerializeField] private float raisedSpriteYPosition = 2.3f; // Y position when sprite is raised

    private MaterialData materialData; // Current material in the cauldron
    private int currentSmashedCount; // Number of times the material has been stirred
    private Vector3 initialSpritePosition; // Initial position of the sprite
    private List<MaterialData> craftedMaterialDataList = new List<MaterialData>();

    private void Start()
    {
        if (droppedMaterialSprite == null)
        {
            enabled = false;
            return;
        }

        initialSpritePosition = droppedMaterialSprite.transform.localPosition;
        ResetCauldron();
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnMaterialStirred += StirMaterial;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnMaterialStirred -= StirMaterial;

    }

    /// <summary>
    /// Sets the material data for the dropped material.
    /// </summary>
    /// <param name="materialData">Material data to set.</param>
    public void SetDroppedMaterial(MaterialData materialData)
    {
        if (materialData == null) return;

        this.materialData = materialData;
        currentSmashedCount = 0;
        RaiseDroppedMaterialSprite();
        HandleMaterialDropped();
        UpdateDroppedMaterialSprite(CombineColors());
    }

    /// <summary>
    /// Resets the cauldron to its initial state.
    /// </summary>
    private void ResetCauldron()
    {
        materialData = null;
        currentSmashedCount = 0;
        craftedMaterialDataList.Clear();
        craftingToolMaterialUI.Hide();
        craftingToolProgressUI.UpdateProgressBar(0);
    }

    /// <summary>
    /// Simulates stirring the material in the cauldron.
    /// </summary>
    public void StirMaterial()
    {
        if (materialData == null) return;

        if (currentSmashedCount < materialData.SmashedTimes - 1)
        {
            currentSmashedCount++;
            UpdateStirredSprite();
        }
        else
        {
            CraftPotion();
            LowerDroppedMaterialSprite();
            ResetCauldron();
        }
    }

    /// <summary>
    /// Updates the sprite color based on the material.
    /// </summary>
    /// <param name="color">The color to set on the sprite.</param>
    private void UpdateDroppedMaterialSprite(Color color)
    {
        droppedMaterialSprite.color = color;
    }

    private Color CombineColors()
    {
        Color result = Color.black;

        foreach (MaterialData item in craftedMaterialDataList)
        {
            result += item.Color;
        }

        result /= craftedMaterialDataList.Count; // Averaging
        return result;
    }

    /// <summary>
    /// Updates the sprite to reflect the current smashed state.
    /// </summary>
    private void UpdateStirredSprite()
    {
        if (gameAssetSO.StirredMaterialSprites == null || gameAssetSO.StirredMaterialSprites.Count == 0)
        {
            return;
        }

        craftingToolProgressUI.UpdateProgressBar(currentSmashedCount / (float)materialData.SmashedTimes);

        int totalSprites = gameAssetSO.StirredMaterialSprites.Count;
        int rangePerSprite = Mathf.CeilToInt((float)materialData.SmashedTimes / totalSprites);

        int spriteIndex = Mathf.Clamp(currentSmashedCount / rangePerSprite, 0, totalSprites - 1);
        droppedMaterialSprite.sprite = gameAssetSO.StirredMaterialSprites[spriteIndex];
    }

    /// <summary>
    /// Raises the sprite to indicate progress.
    /// </summary>
    private void RaiseDroppedMaterialSprite()
    {
        droppedMaterialSprite.transform.DOLocalMoveY(raisedSpriteYPosition, 0.5f);
    }

    /// <summary>
    /// Lowers the sprite back to its initial position.
    /// </summary>
    private void LowerDroppedMaterialSprite()
    {
        droppedMaterialSprite.transform.DOLocalMoveY(initialSpritePosition.y, 0.5f);
    }

    /// <summary>
    /// Crafts the material and invokes the relevant event.
    /// </summary>
    private void HandleMaterialDropped()
    {
        if (playerEventSO != null && materialData != null)
        {
            AudioManager.Instance.PlayMaterialFallsIntoCauldronSounds();

            craftedMaterialDataList.Add(materialData);
            StartCoroutine(craftingToolMaterialUI.RefreshUI(craftedMaterialDataList));
            playerEventSO.Event.OnMaterialGetInCauldron?.Invoke();
        }
        else
        {
            Debug.LogWarning("CraftMaterial invoked without proper setup.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SmashedMaterialMovement materialMovement))
        {
            SetDroppedMaterial(materialMovement.MaterialData);
            Destroy(collision.gameObject);
        }
    }

    private void CraftPotion()
    {
        List<MaterialType> materialTypeList = new List<MaterialType>();
        foreach (var item in craftedMaterialDataList)
        {
            materialTypeList.Add(item.MaterialType);
        }

        ShopCraftingManager.Instance.HandlePotionCrafted(materialTypeList);
    }
}
