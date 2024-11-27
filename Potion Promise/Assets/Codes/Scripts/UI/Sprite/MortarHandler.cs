using UnityEngine;

public class MortarHandler : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private GameAssetSO gameAssetSO;

    [Header("UI Components")]
    [SerializeField] private SpriteRenderer droppedMaterialSprite;
    [SerializeField] private SpriteRenderer smashedMaterialSprite;

    private MaterialData materialData;
    private int currentSmashedCount;

    private void Start()
    {
        ResetMortarUI();
    }

    /// <summary>
    /// Initializes the dropped material in the mortar.
    /// </summary>
    /// <param name="materialData">The material data to set.</param>
    public void SetDroppedMaterial(MaterialData materialData)
    {
        if (materialData == null)
        {
            Debug.LogWarning("SetDroppedMaterial called with null MaterialData.");
            return;
        }

        this.materialData = materialData;
        currentSmashedCount = 0;

        droppedMaterialSprite.gameObject.SetActive(true);
        smashedMaterialSprite.gameObject.SetActive(false);

        droppedMaterialSprite.color = materialData.Color;
        UpdateSmashedSprite();
    }

    /// <summary>
    /// Smashes the material and updates the UI accordingly.
    /// </summary>
    public void SmashMaterial()
    {
        if (materialData == null)
        {
            return;
        }

        if (currentSmashedCount < materialData.SmashedTimes - 1)
        {
            currentSmashedCount++;
            UpdateSmashedSprite();
        }
        else
        {
            SpawnSmashedMaterial();
            DisplayFinalSmashedSprite();
            ResetMortar();
        }
    }

    /// <summary>
    /// Updates the sprite to reflect the current smashed state.
    /// </summary>
    private void UpdateSmashedSprite()
    {
        if (gameAssetSO.SmashedMaterialSprites == null || gameAssetSO.SmashedMaterialSprites.Count == 0)
        {
            Debug.LogWarning("No smashed material sprites available in GameAssetSO.");
            return;
        }

        int totalSprites = gameAssetSO.SmashedMaterialSprites.Count;
        int rangePerSprite = Mathf.CeilToInt((float)materialData.SmashedTimes / totalSprites);

        int spriteIndex = Mathf.Clamp(currentSmashedCount / rangePerSprite, 0, totalSprites - 1);
        droppedMaterialSprite.sprite = gameAssetSO.SmashedMaterialSprites[spriteIndex];
    }

    /// <summary>
    /// Displays the final smashed material sprite and updates the UI.
    /// </summary>
    private void DisplayFinalSmashedSprite()
    {
        droppedMaterialSprite.gameObject.SetActive(false);
        smashedMaterialSprite.gameObject.SetActive(true);
        smashedMaterialSprite.color = materialData.Color;
    }

    /// <summary>
    /// Spawns the smashed material as a separate game object.
    /// </summary>
    private void SpawnSmashedMaterial()
    {
        if (smashedMaterialSprite == null)
        {
            Debug.LogWarning("SmashedMaterialSprite is not assigned.");
            return;
        }

        GameObject spawnedObject = Instantiate(smashedMaterialSprite.gameObject, smashedMaterialSprite.transform.position, Quaternion.identity, transform);
        spawnedObject.SetActive(true);

        if (spawnedObject.TryGetComponent<SmashedMaterialMovement>(out var smashedMaterialMovement))
        {
            smashedMaterialMovement.Initialize(materialData);
        }
        else
        {
            Debug.LogWarning("Spawned object is missing SmashedMaterialMovement component.");
        }
    }

    /// <summary>
    /// Resets the mortar's state and hides UI elements.
    /// </summary>
    private void ResetMortar()
    {
        materialData = null;
        currentSmashedCount = 0;
        ResetMortarUI();
    }

    /// <summary>
    /// Resets the mortar UI to its initial state.
    /// </summary>
    private void ResetMortarUI()
    {
        droppedMaterialSprite.gameObject.SetActive(false);
        smashedMaterialSprite.gameObject.SetActive(false);
    }
}
