using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class RecipeBookUI : BaseUI
{
    [Header("Project References")]
    [SerializeField] private PotionDatabaseSO potionDatabaseSO;
    [SerializeField] private MaterialDatabaseSO materialDatabaseSO;
    [SerializeField] private GameAssetSO gameAssetSO;

    [Header("Scene References")]
    [SerializeField] private TMP_Text pageTitleText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button potionTabButton;
    [SerializeField] private Button materialTabButton;

    public GameAssetSO GameAssetSO => gameAssetSO;
    public MaterialDatabaseSO MaterialDatabaseSO => materialDatabaseSO;

    private void Awake()
    {
        closeButton.onClick.AddListener(HandleOnClose);

        potionTabButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();

            playerEventSO.Event.OnAnyPageUIClosed?.Invoke();
            playerEventSO.Event.OnPotionPageTabButtonClicked?.Invoke();
            potionTabButton.GetComponent<RecipeBookTabUI>().SetSelected(true);
            materialTabButton.GetComponent<RecipeBookTabUI>().SetSelected(false);
            pageTitleText.text = "POTIONS";
        });

        materialTabButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();

            playerEventSO.Event.OnAnyPageUIClosed?.Invoke();
            playerEventSO.Event.OnMaterialTabButtonClicked?.Invoke();
            potionTabButton.GetComponent<RecipeBookTabUI>().SetSelected(false);
            materialTabButton.GetComponent<RecipeBookTabUI>().SetSelected(true);
            pageTitleText.text = "MATERIALS";
        });

        playerEventSO.Event.OnRecipeBookPageInitialized.Invoke(potionDatabaseSO, materialDatabaseSO, gameAssetSO);
    }

    private void Start()
    {
        InstantHide();
    }

    protected override void OnEnable()
    {
        playerEventSO.Event.OnRecipeBookOpened += OpenDefaultPage;
    }

    protected override void OnDisable()
    {
        playerEventSO.Event.OnRecipeBookOpened -= OpenDefaultPage;
    }

    private void OpenDefaultPage()
    {
        Show();

        // Default Page To Open
        OpenPotionPage();
    }

    private void OpenPotionPage()
    {
        playerEventSO.Event.OnAnyPageUIClosed?.Invoke();
        playerEventSO.Event.OnPotionPageTabButtonClicked?.Invoke();
        pageTitleText.text = "POTIONS";
    }

    private void HandleOnClose()
    {
        AudioManager.Instance.PlayClickSound();
        Hide();
    }
}
