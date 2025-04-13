using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameSceneManager : PersistentSingleton<GameSceneManager>
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

    private SceneAsset currentScene;

    public SceneAsset CurrentScene => currentScene;

    private void Start()
    {
        currentScene = mainMenu;
    }

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

    public void LoadMainMenuScene()
    {
        currentScene = mainMenu;
        LoadNormalLoadingScreenSlide(currentScene);
    }

    public void LoadCutsceneScene(bool isDayStart = false)
    {
        currentScene = cutScene;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(currentScene);
            return;
        }
        LoadNormalLoadingScreenSlide(currentScene);
    }

    public void LoadVisualNovelScene(bool isDayStart = false)
    {
        currentScene = visualNovel;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(currentScene);
            return;
        }
        LoadNormalLoadingScreenSlide(currentScene);
    }

    public void LoadShopScene(bool isDayStart = false)
    {
        currentScene = shop;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(currentScene);
            return;
        }
        LoadNormalLoadingScreenSlide(currentScene);
    }

    public void LoadGatheringScene(bool isDayStart = false)
    {
        currentScene = gathering;
        if (isDayStart)
        {
            LoadDayStartLoadingScreenSlide(currentScene);
            return;
        }
        LoadNormalLoadingScreenSlide(currentScene);
    }

    public void LoadCreditScene(bool isDayStart = false)
    {
        LoadNormalLoadingScreenFade(creditScene);
    }
}
