using System;
using System.Collections;
using System.Collections.Generic;
using Eggtato.Utility;
using UnityEngine;

public class DayProgressionManager : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;
    private GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = GameDataManager.Instance;
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnGoToNextScene += CheckForCurrentProgressionDay;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnGoToNextScene -= CheckForCurrentProgressionDay;
    }

    private void CheckForCurrentProgressionDay()
    {
        ProgressCurrentDay(gameDataManager.CurrentDay);
    }

    public void ProgressCurrentDay(int currentDay)
    {
        switch (currentDay)
        {
            case 1:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 2:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 3:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 4:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 5:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 6:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 7:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 8:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 9:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 10:
                GameSceneManager.Instance.LoadShopScene();
                break;
            case 11:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 12:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 13:
                GameSceneManager.Instance.LoadVisualNovelScene(); //VisualNovel
                break;
            case 14:
                GameSceneManager.Instance.LoadGatheringScene();
                break;
            case 15:
                Time.timeScale = 1;
                break;
        }


        GameDataManager.Instance.IncreaseCurrentDay();
    }
}
