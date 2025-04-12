using UnityEngine;
using DG.Tweening;
using System;

public class MortarHandler : MonoBehaviour
{
    [Header("Project Reference")]
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private PlayerEventSO playerEventSO;


    [Header("Scene Reference")]
    [SerializeField] private SpriteRenderer droppedMaterialSprite;
    [SerializeField] private SpriteRenderer smashedMaterialSprite;
    [SerializeField] private CraftingToolMaterialUI craftingToolMaterialUI;
    [SerializeField] private CraftingToolMaterialProgressUI craftingToolProgressUI;

    [Header("Configs")]
    [SerializeField] private float raisedSpriteYPosition = 1.4f; // Y position when sprite is raised

    private MaterialData materialData;
    private int currentSmashedCount;
    private Vector3 initialSpritePosition; // Initial position of the sprite
    private bool hasSmashedMaterial = false;
    private BoxCollider2D boxCollider2D;
    private GameObject droppedMaterialGameObject;

    public PlayerEventSO PlayerEventSO => playerEventSO;

    private void Start()
    {
        initialSpritePosition = droppedMaterialSprite.transform.localPosition;
        ResetMortarUI();
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnMaterialSmashed += SmashMaterial;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnMaterialSmashed -= SmashMaterial;

    }

    /// <summary>
    /// Initializes the dropped material in the mortar.
    /// </summary>
    /// <param name="materialData">The material data to set.</param>
    public void SetDroppedMaterial()
    {
        droppedMaterialSprite.gameObject.SetActive(true);
        smashedMaterialSprite.gameObject.SetActive(false);

        droppedMaterialSprite.color = materialData.Color;
        UpdateSmashedSprite();
        RaiseDroppedMaterialSprite();

        craftingToolMaterialUI.RefreshUI(materialData);

        Destroy(droppedMaterialGameObject);
    }

    private void RaiseDroppedMaterialSprite()
    {
        droppedMaterialSprite.transform.DOLocalMoveY(raisedSpriteYPosition, 0.5f);
    }

    private void LowerDroppedMaterialSprite()
    {
        droppedMaterialSprite.transform.DOLocalMoveY(initialSpritePosition.y, 0.5f);
    }

    /// <summary>
    /// Smashes the material and updates the UI accordingly.
    /// </summary>
    public void SmashMaterial()
    {
        if (!hasSmashedMaterial)
        {
            return;
        }

        if (currentSmashedCount <= 0)
        {
            SetDroppedMaterial();
        }


        if (currentSmashedCount < materialData.SmashedTimes - 1)
        {
            currentSmashedCount++;
            UpdateSmashedSprite();
        }
        else
        {
            SpawnSmashedMaterial();
            ResetMortar();
            LowerDroppedMaterialSprite();
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

        craftingToolProgressUI.UpdateProgressBar(currentSmashedCount / (float)materialData.SmashedTimes);

        int totalSprites = gameAssetSO.SmashedMaterialSprites.Count;
        int rangePerSprite = Mathf.CeilToInt((float)materialData.SmashedTimes / totalSprites);

        int spriteIndex = Mathf.Clamp(currentSmashedCount / rangePerSprite, 0, totalSprites - 1);
        droppedMaterialSprite.sprite = gameAssetSO.SmashedMaterialSprites[spriteIndex];
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
        hasSmashedMaterial = false;
        ResetMortarUI();
    }

    /// <summary>
    /// Resets the mortar UI to its initial state.
    /// </summary>
    private void ResetMortarUI()
    {
        droppedMaterialSprite.gameObject.SetActive(false);
        smashedMaterialSprite.gameObject.SetActive(false);
        craftingToolMaterialUI.Hide();
        craftingToolProgressUI.UpdateProgressBar(0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DroppedMaterialMovement>(out DroppedMaterialMovement droppedMaterial))
        {
            materialData = droppedMaterial.MaterialData;
            droppedMaterialGameObject = droppedMaterial.gameObject;
            hasSmashedMaterial = true;
        }
    }
}
