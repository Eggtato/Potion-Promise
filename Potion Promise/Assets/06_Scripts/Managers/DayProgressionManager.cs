using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GameSceneManager))]
public class DayProgressionManager : MonoBehaviour
{
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private DayProgressionSO dayProgressionSO;

    private GameDataManager gameDataManager;
    private GameSceneManager gameSceneManager;
    private ProgressionType? lastCompletedProgressionType;

    private void Awake()
    {
        gameDataManager = GameDataManager.Instance;
        gameSceneManager = GetComponent<GameSceneManager>();
    }

    private void Start()
    {
        SavePreviousProgression(); // Ensure we save before progressing
    }

    private void OnEnable()
    {
        playerEventSO.Event.OnGoToNextScene += HandleProgressionBeforeSceneChange;
        playerEventSO.Event.OnDayEnd += SavePreviousProgression;
    }

    private void OnDisable()
    {
        playerEventSO.Event.OnGoToNextScene -= HandleProgressionBeforeSceneChange;
        playerEventSO.Event.OnDayEnd -= SavePreviousProgression;
    }

    private void HandleProgressionBeforeSceneChange()
    {
        // Before changing the scene, ensure we save the last completed progression
        SavePreviousProgression();

        // Now proceed to handle the next progression
        ProcessCurrentProgression();
    }

    private void SavePreviousProgression()
    {
        if (lastCompletedProgressionType == null)
        {
            Debug.Log("No previous progression to save.");
            return;
        }

        int currentDay = gameDataManager.CurrentDay;

        ProgressionData savedData = gameDataManager.ProgressionDataList
            .FirstOrDefault(i => i.Day == currentDay);

        if (savedData != null && savedData.ProgressionTypes.Contains(lastCompletedProgressionType.Value))
        {
            Debug.Log($"Previous progression ({lastCompletedProgressionType}) already saved.");
            return;
        }

        // Save the last completed progression before changing the scene
        gameDataManager.AddNewProgression(currentDay, lastCompletedProgressionType.Value);
        Debug.Log($"Saved previous progression type: {lastCompletedProgressionType}");

        lastCompletedProgressionType = null; // Reset to avoid double saving
    }

    private void ProcessCurrentProgression()
    {
        int currentDay = gameDataManager.CurrentDay;
        List<ProgressionData> dataList = dayProgressionSO.DayProgressionDataList;
        List<ProgressionData> savedDataList = gameDataManager.ProgressionDataList;

        ProgressionData data = dataList.FirstOrDefault(i => i.Day == currentDay);
        if (data == null)
        {
            Debug.LogError($"No progression data found for day {currentDay}.");
            return;
        }

        ProgressionData savedData = savedDataList.FirstOrDefault(i => i.Day == currentDay);
        bool allPartsCompleted = ProcessIncompleteParts(currentDay, data, savedData);

        if (allPartsCompleted)
        {
            gameDataManager.IncreaseCurrentDay();
            ProcessCurrentProgression();
        }
    }

    private bool ProcessIncompleteParts(int currentDay, ProgressionData data, ProgressionData savedData)
    {
        foreach (var progressionType in data.ProgressionTypes)
        {
            if (savedData == null || !savedData.ProgressionTypes.Contains(progressionType))
            {
                // Store the progression, but don't save it yet
                lastCompletedProgressionType = progressionType;

                // Load the next scene
                LoadSceneForProgression(currentDay, progressionType);
                return false;
            }
        }

        return true; // All parts for the day are completed
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
