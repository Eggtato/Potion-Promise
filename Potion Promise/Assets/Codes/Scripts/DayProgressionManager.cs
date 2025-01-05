using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eggtato.Utility;
using UnityEngine;

[RequireComponent(typeof(GameSceneManager))]
public class DayProgressionManager : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private DayProgressionSO dayProgressionSO;

    private GameDataManager gameDataManager;
    private GameSceneManager gameSceneManager;

    private void Awake()
    {
        gameDataManager = GameDataManager.Instance;
        gameSceneManager = GetComponent<GameSceneManager>();
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnGoToNextScene += CheckForCurrentProgressionDay;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnGoToNextScene -= CheckForCurrentProgressionDay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckForCurrentProgressionDay();
        }
    }

    private void CheckForCurrentProgressionDay()
    {
        DoProgress(gameDataManager.CurrentDay);
    }

    public void DoProgress(int currentDay)
    {
        List<ProgressionData> dataList = dayProgressionSO.DayProgressionDataList;
        List<ProgressionData> savedDataList = GameDataManager.Instance.ProgressionDataList;

        ProgressionData data = dataList.Find(i => i.Day == currentDay);
        ProgressionData savedData = savedDataList.Find(i => i.Day == currentDay);

        if (savedData == null)
        {
            // First Time
            GameDataManager.Instance.AddNewProgression(currentDay, data.ProgressionTypes[0]);
            switch (data.ProgressionTypes[0])
            {
                case ProgressionType.EarlyVisualNovel:
                    CrossSceneMessage.Send(currentDay.ToString(), data.ProgressionTypes[0]);
                    gameSceneManager.LoadVisualNovelScene();
                    break;
                case ProgressionType.Shop:
                    gameSceneManager.LoadShopScene();
                    break;
                case ProgressionType.MiddleVisualNovel:
                    CrossSceneMessage.Send(currentDay.ToString(), data.ProgressionTypes[0]);
                    gameSceneManager.LoadVisualNovelScene();
                    break;
                case ProgressionType.Gathering:
                    gameSceneManager.LoadGatheringScene();
                    break;
                case ProgressionType.EndVisualNovel:
                    CrossSceneMessage.Send(currentDay.ToString(), data.ProgressionTypes[0]);
                    gameSceneManager.LoadVisualNovelScene();
                    break;
            }
            return;
        }

        bool isAllPartInDayHasBeenDone = true;
        for (int i = 0; i < data.ProgressionTypes.Count; i++)
        {
            if (savedData.ProgressionTypes.Contains(data.ProgressionTypes[i]))
            {
                continue;
            }
            else
            {
                isAllPartInDayHasBeenDone = false;

                GameDataManager.Instance.AddNewProgression(currentDay, data.ProgressionTypes[i]);

                switch (data.ProgressionTypes[i])
                {
                    case ProgressionType.EarlyVisualNovel:
                        CrossSceneMessage.Send(currentDay.ToString(), data.ProgressionTypes[i]);
                        gameSceneManager.LoadVisualNovelScene();
                        return;
                    case ProgressionType.Shop:
                        Debug.Log("Load Shop");
                        gameSceneManager.LoadShopScene();
                        return;
                    case ProgressionType.MiddleVisualNovel:
                        CrossSceneMessage.Send(currentDay.ToString(), data.ProgressionTypes[i]);
                        gameSceneManager.LoadVisualNovelScene();
                        return;
                    case ProgressionType.Gathering:
                        gameSceneManager.LoadGatheringScene();
                        return;
                    case ProgressionType.EndVisualNovel:
                        CrossSceneMessage.Send(currentDay.ToString(), data.ProgressionTypes[i]);
                        gameSceneManager.LoadVisualNovelScene();
                        return;
                }
            }
        }

        if (isAllPartInDayHasBeenDone)
        {
            GameDataManager.Instance.IncreaseCurrentDay();
            CheckForCurrentProgressionDay();
        }

    }


}
