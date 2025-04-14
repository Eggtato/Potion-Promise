using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;
using UnityEngine.SceneManagement;
// using UnityEditor;

public class GameSceneManager : PersistentSingleton<GameSceneManager>
{
    [Header("Scene Names")]
    [SerializeField] private string mainMenu;
    [SerializeField] private string cutScene;
    [SerializeField] private string visualNovel;
    [SerializeField] private string shop;
    [SerializeField] private string gathering;
    [SerializeField] private string creditScene;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player loadNormalLoadingScreenSlide;
    [SerializeField] private MMF_Player loadDayStartLoadingScreenSlide;
    [SerializeField] private MMF_Player loadNormalLoadingScreenFade;

    private string currentScene;

    public string CurrentScene => currentScene;

    private void Start()
    {
        currentScene = mainMenu;
    }

    private void LoadNormalLoadingScreenSlide(string scene)
    {
        MMF_LoadScene loadScene = loadNormalLoadingScreenSlide.GetFeedbackOfType<MMF_LoadScene>();
        loadScene.DestinationSceneName = scene;
        loadNormalLoadingScreenSlide.PlayFeedbacks();
    }

    private void LoadDayStartLoadingScreenSlide(string scene)
    {
        MMF_LoadScene loadScene = loadDayStartLoadingScreenSlide.GetFeedbackOfType<MMF_LoadScene>();
        loadScene.DestinationSceneName = scene;
        loadDayStartLoadingScreenSlide.PlayFeedbacks();
    }

    private void LoadNormalLoadingScreenFade(string scene)
    {
        MMF_LoadScene loadScene = loadNormalLoadingScreenFade.GetFeedbackOfType<MMF_LoadScene>();
        loadScene.DestinationSceneName = scene;
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
