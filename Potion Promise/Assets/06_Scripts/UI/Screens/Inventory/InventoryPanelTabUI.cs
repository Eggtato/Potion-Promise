using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelTabUI : MonoBehaviour
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
                buttonImage.sprite = isSelected ? gameAssetSO.FilterUpOn1 : gameAssetSO.FilterUpOff1;
                break;
            case InventoryTabType.Material:
                buttonImage.sprite = isSelected ? gameAssetSO.FilterUpOn2 : gameAssetSO.FilterUpOff2;
                break;
        }
    }
}
