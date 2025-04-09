using UnityEngine;
using UnityEngine.UI;

public class RecipeBookTabUI : MonoBehaviour
{
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private InventoryTabType tabType;

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void SetSelected(bool isSelected)
    {
        switch (tabType)
        {
            case InventoryTabType.Potion:
                buttonImage.sprite = isSelected ? gameAssetSO.bookMarkOn : gameAssetSO.bookMarkOff;
                break;
            case InventoryTabType.Material:
                buttonImage.sprite = isSelected ? gameAssetSO.bookMarkOn : gameAssetSO.bookMarkOff;
                break;
        }
    }
}
