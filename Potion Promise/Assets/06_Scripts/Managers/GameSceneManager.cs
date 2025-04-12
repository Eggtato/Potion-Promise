using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameSceneManager : Singleton<GameSceneManager>
{
    [Header("Scene Names")]
    [SerializeField] private SceneAsset mainMenu;
    [SerializeField] private SceneAsset cutScene;
    [SerializeField] private SceneAsset visualNovel;
    [SerializeField] private SceneAsset shop;
    [SerializeField] private SceneAsset gathering;
    [SerializeField] private SceneAsset creditScene;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player loadNormalLoadingScreenSlide;
    [SerializeField] private MMF_Player loadDayStartLoadingScreenSlide;
    [SerializeField] private MMF_Player loadNormalLoadingScreenFade;

    [SerializeField] private MMFeedbacks loadMainMenuSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadCutsceneSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadVisualNovelSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadShopSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadGatheringSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadCreditSceneFeedbacks;

    private void LoadNormalLoadingScreenSlide(SceneAsset scene)
    {
        MMF_LoadScene loadScene = loadNormalLoadingScreenSlide.GetFeedbackOfType<MMF_LoadScene>();
        loadScene.DestinationSceneName = scene.name;
        loadNormalLoadingScreenSlide.PlayFeedbacks();
    }

    private void LoadDayStartLoadingScreenSlide(SceneAsset scene)
    {
        MMF_LoadScene loadScene = loadDayStartLoadingScreenSlide.GetFeedbackOfType<MMF_LoadScene>();
        loadScene.DestinationSceneName = scene.name;
        loadDayStartLoadingScreenSlide.PlayFeedbacks();
    }

    private void LoadNormalLoadingScreenFade(SceneAsset scene)
    {
        MMF_LoadScene loadScene = loadNormalLoadingScreenFade.GetFeedbackOfType<MMF_LoadScene>();
        loadScene.DestinationSceneName = scene.name;
        loadNormalLoadingScreenFade.PlayFeedbacks();
    }

    public void LoadMainMenuScene(bool isDayStart = false)
    {
        SceneAsset scene = mainMenu;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(scene);
            return;
        }
        LoadNormalLoadingScreenSlide(scene);
    }

    public void LoadCutsceneScene(bool isDayStart = false)
    {
        SceneAsset scene = cutScene;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(scene);
            return;
        }
        LoadNormalLoadingScreenSlide(scene);
    }

    public void LoadVisualNovelScene(bool isDayStart = false)
    {
        SceneAsset scene = visualNovel;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(scene);
            return;
        }
        LoadNormalLoadingScreenSlide(scene);
    }

    public void LoadShopScene(bool isDayStart = false)
    {
        SceneAsset scene = shop;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(scene);
            return;
        }
        LoadNormalLoadingScreenSlide(scene);
    }

    public void LoadGatheringScene(bool isDayStart = false)
    {
        SceneAsset scene = gathering;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(scene);
            return;
        }
        LoadNormalLoadingScreenSlide(scene);
    }

    public void LoadCreditScene(bool isDayStart = false)
    {
        LoadNormalLoadingScreenFade(creditScene);
    }
}
