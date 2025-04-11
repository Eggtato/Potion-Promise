using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class CraftingToolMaterialUI : MonoBehaviour
{
    public enum MaterialSizeType
    {
        Single,
        Triple
    }

    [SerializeField] private MaterialSizeType materialSizeType;

    [Header("Project Reference")]
    [SerializeField] private GameSettingSO gameSettingSO;
    [SerializeField] private GameAssetSO gameAssetSO;

    [Header("UI Reference")]
    [SerializeField] private CanvasGroup panel;
    [ShowIf("materialSizeType", MaterialSizeType.Single)][SerializeField] private Image materialImage;
    [ShowIf("materialSizeType", MaterialSizeType.Triple)][SerializeField] private Image[] materialImages;
    [ShowIf("materialSizeType", MaterialSizeType.Triple)][SerializeField] private Color32 emptySlotColor;

    [Header("Animation")]
    [SerializeField] private Ease materialAppearEase = Ease.OutBounce;

    private void Start()
    {
        Hide();
    }

    public void RefreshUI(MaterialData materialData)
    {
        materialImage.transform.localScale = Vector3.zero;
        materialImage.sprite = materialData.Sprite;

        panel.gameObject.SetActive(true);
        panel.DOFade(1, gameSettingSO.CraftingMaterialFadeInAnimation).OnComplete(() =>
        {
            materialImage.transform.DOScale(1, gameSettingSO.CraftingMaterialFadeInAnimation).SetEase(materialAppearEase);
        });
    }

    public IEnumerator RefreshUI(List<MaterialData> materialDatas)
    {
        for (int i = 0; i < materialImages.Length; i++)
        {
            if (materialImages[i].sprite == gameAssetSO.EmptyMaterialSlot)
            {
                materialImages[i].transform.localScale = Vector3.zero;
                materialImages[i].transform.DOScale(1, gameSettingSO.CraftingMaterialFadeInAnimation).SetEase(materialAppearEase);

                if (i < materialDatas.Count)
                {
                    materialImages[i].sprite = materialDatas[i].Sprite;
                    materialImages[i].color = Color.white;
                }

                if (!panel.gameObject.activeInHierarchy)
                {
                    panel.gameObject.SetActive(true);
                    panel.DOFade(1, gameSettingSO.CraftingMaterialFadeInAnimation);
                }
                yield return new WaitForSeconds(gameSettingSO.FadeInAnimation);
            }
        }
    }

    public void Hide()
    {
        panel.DOFade(0, gameSettingSO.CraftingMaterialFadeInAnimation).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });

        if (materialSizeType != MaterialSizeType.Triple) return;

        foreach (var item in materialImages)
        {
            item.sprite = gameAssetSO.EmptyMaterialSlot;
            item.color = emptySlotColor;
        }
    }
}
