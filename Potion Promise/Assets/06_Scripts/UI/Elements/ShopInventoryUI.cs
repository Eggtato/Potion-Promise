using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopInventoryUI : MonoBehaviour
{
    [Header("Database Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;
    [SerializeField] private MaterialDatabaseSO materialDatabaseSO;
    [SerializeField] private GameSettingSO gameSettingSO;

    [Header("UI Elements")]
    [SerializeField] private Button potionPanelButton;
    [SerializeField] private Button materialPanelButton;
    [SerializeField] private CanvasGroup potionPanelCanvas;
    [SerializeField] private CanvasGroup materialPanelCanvas;

    [Header("Inventory Templates")]
    [SerializeField] private InventoryPotionSlotUI potionSlotTemplate;
    [SerializeField] private Transform potionInventoryParent;

    [SerializeField] private InventoryMaterialSlotUI materialSlotTemplate;
    [SerializeField] private Transform materialInventoryParent;

    private void Awake()
    {
        potionPanelButton.onClick.AddListener(() =>
        {
            ShowPotionPanel();
            TogglePanels(potionPanelCanvas, materialPanelCanvas, true);
        });
        materialPanelButton.onClick.AddListener(() =>
        {
            ShowMaterialPanel();
            TogglePanels(materialPanelCanvas, potionPanelCanvas, true);
        });
    }

    private void Start()
    {
        ShowPotionPanel();
        TogglePanels(potionPanelCanvas, materialPanelCanvas);
    }

    private void OnEnable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnPotionInventoryChanged += ShowPotionPanel;
            playerEventSO.Event.OnMaterialInventoryChanged += ShowMaterialPanel;
        }
    }

    private void OnDisable()
    {
        if (playerEventSO?.Event != null)
        {
            playerEventSO.Event.OnPotionInventoryChanged -= ShowPotionPanel;
            playerEventSO.Event.OnMaterialInventoryChanged -= ShowMaterialPanel;
        }
    }

    private void ShowPotionPanel()
    {
        GenerateInventory(
            GameDataManager.Instance?.CraftedPotionDataList,
            potionSlotTemplate,
            potionInventoryParent,
            data => potionDatabaseSO.GetPotionData(data.PotionType),
            (slot, data, db) => slot.Initialize(data, db)
        );

        potionPanelButton.GetComponent<InventoryPanelTabUI>().SetSelected(true);
        materialPanelButton.GetComponent<InventoryPanelTabUI>().SetSelected(false);


    }

    private void ShowMaterialPanel()
    {
        GenerateInventory(
            GameDataManager.Instance?.ObtainedMaterialDataList,
            materialSlotTemplate,
            materialInventoryParent,
            data => materialDatabaseSO.GetMaterialData(data.MaterialType),
            (slot, data, db) => slot.Initialize(data, db)
        );

        materialPanelButton.GetComponent<InventoryPanelTabUI>().SetSelected(true);
        potionPanelButton.GetComponent<InventoryPanelTabUI>().SetSelected(false);

    }

    private void TogglePanels(CanvasGroup toShow, CanvasGroup toHide, bool withFade = false)
    {
        toShow.gameObject.SetActive(true);

        if (withFade)
        {
            toShow.DOFade(0, 0).OnComplete(() =>
            {
                toShow.DOFade(1, gameSettingSO.FadeInAnimation);
            });

            toHide.DOFade(0, gameSettingSO.FadeInAnimation).OnComplete(() =>
            {
                toHide.gameObject.SetActive(false);
            });
        }
        else
        {
            toHide.gameObject.SetActive(false);
        }
    }

    private void GenerateInventory<TData, TSlotUI, TDatabaseData>(
        List<TData> dataList,
        TSlotUI template,
        Transform parent,
        System.Func<TData, TDatabaseData> getDatabaseData,
        System.Action<TSlotUI, TData, TDatabaseData> initializeSlot
    ) where TSlotUI : MonoBehaviour
    {
        // Cleanup first
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        if (dataList == null || dataList.Count == 0) return;

        foreach (var data in dataList)
        {
            TSlotUI slot = Instantiate(template, parent);
            slot.gameObject.SetActive(true);

            var dbData = getDatabaseData.Invoke(data);
            if (dbData == null)
            {
                Debug.LogWarning($"Database data not found for: {data}");
                continue;
            }

            initializeSlot.Invoke(slot, data, dbData);
        }
    }
}
