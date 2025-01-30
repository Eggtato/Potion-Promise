using System;
using System.Collections.Generic;
using System.Linq;
using Eggtato.Utility;
using Unity.VisualScripting;
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
        HandleDayProgression(gameDataManager.CurrentDay);
    }

    public void HandleDayProgression(int currentDay)
    {
        List<ProgressionData> dataList = dayProgressionSO.DayProgressionDataList;
        List<ProgressionData> savedDataList = gameDataManager.ProgressionDataList;

        ProgressionData data = dataList.FirstOrDefault(i => i.Day == currentDay);
        if (data == null)
        {
            Debug.LogError($"No progression data found for day {currentDay}.");
            return;
        }

        ProgressionData savedData = savedDataList.FirstOrDefault(i => i.Day == currentDay);

        if (savedData == null)
        {
            StartNewProgression(currentDay, data.ProgressionTypes[0]);
            return;
        }

        bool allPartsCompleted = ProcessIncompleteParts(currentDay, data, savedData);

        if (allPartsCompleted)
        {
            gameDataManager.IncreaseCurrentDay();
            CheckForCurrentProgressionDay();
        }
    }

    private void StartNewProgression(int currentDay, ProgressionType progressionType)
    {
        gameDataManager.AddNewProgression(currentDay, progressionType);
        LoadSceneForProgression(currentDay, progressionType);
    }

    private bool ProcessIncompleteParts(int currentDay, ProgressionData data, ProgressionData savedData)
    {
        foreach (var progressionType in data.ProgressionTypes)
        {
            if (!savedData.ProgressionTypes.Contains(progressionType))
            {
                gameDataManager.AddNewProgression(currentDay, progressionType);
                LoadSceneForProgression(currentDay, progressionType);
                return false;
            }
        }

        return true; // All parts for the day are completed.
    }

    private void LoadSceneForProgression(int currentDay, ProgressionType progressionType)
    {
        CrossSceneMessage.Send(currentDay.ToString(), progressionType);

        switch (progressionType)
        {
            case ProgressionType.CutScene:
                gameSceneManager.LoadCutsceneScene();
                break;
            case ProgressionType.EarlyVisualNovel:
            case ProgressionType.MiddleVisualNovel:
            case ProgressionType.EndVisualNovel:
                gameSceneManager.LoadVisualNovelScene();
                break;
            case ProgressionType.Shop:
                gameSceneManager.LoadShopScene();
                break;
            case ProgressionType.Gathering:
                gameSceneManager.LoadGatheringScene();
                break;
            case ProgressionType.Credit:
                gameSceneManager.LoadCreditScene();
                break;
            default:
                Debug.LogError($"Unhandled ProgressionType: {progressionType}");
                break;
        }
    }
}
