using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MortarHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer droppedMaterialSprite;

    private MaterialData materialData;

    private void Start()
    {
        droppedMaterialSprite.gameObject.SetActive(false);
    }

    public void SetDroppedMaterial(MaterialData materialData)
    {
        this.materialData = materialData;
        droppedMaterialSprite.gameObject.SetActive(true);
        droppedMaterialSprite.color = materialData.Color;
    }
}
