using System;
using System.Collections.Generic;
using System.Linq;
using Eggtato.Utility;
using UnityEngine;

[RequireComponent(typeof(GameSceneManager))]
public class DayProgressionManager : PersistentSingleton<DayProgressionManager>
{
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private DayProgressionSO dayProgressionSO;

    private GameDataManager gameDataManager;
    private ProgressionType? lastCompletedProgressionType;
    private int lastDay;

    private void Start()
    {
        gameDataManager = GameDataManager.Instance;
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
            return;
        }

        int currentDay = gameDataManager.CurrentDay;

        ProgressionData savedData = gameDataManager.ProgressionDataList
            .FirstOrDefault(i => i.Day == currentDay);

        if (savedData != null && savedData.ProgressionTypes.Contains(lastCompletedProgressionType.Value))
        {
            return;
        }

        // Save the last completed progression before changing the scene
        gameDataManager.AddNewProgression(currentDay, lastCompletedProgressionType.Value);

        lastCompletedProgressionType = null; // Reset to avoid double saving
    }

    private void ProcessCurrentProgression()
    {
        bool isDayStart = false;
        int currentDay = gameDataManager.CurrentDay;
        if (lastDay != 0 && lastDay < currentDay)
        {
            isDayStart = true;
        }
        lastDay = currentDay;
        List<ProgressionData> dataList = dayProgressionSO.DayProgressionDataList;
        List<ProgressionData> savedDataList = gameDataManager.ProgressionDataList;

        ProgressionData data = dataList.FirstOrDefault(i => i.Day == currentDay);
        if (data == null)
        {
            return;
        }

        ProgressionData savedData = savedDataList.FirstOrDefault(i => i.Day == currentDay);
        bool allPartsCompleted = ProcessIncompleteParts(currentDay, data, savedData, isDayStart);

        if (allPartsCompleted)
        {
            gameDataManager.IncreaseCurrentDay();
            ProcessCurrentProgression();
        }
    }

    private bool ProcessIncompleteParts(int currentDay, ProgressionData data, ProgressionData savedData, bool isDayStart)
    {
        foreach (var progressionType in data.ProgressionTypes)
        {
            if (savedData == null || !savedData.ProgressionTypes.Contains(progressionType))
            {
                // Store the progression, but don't save it yet
                lastCompletedProgressionType = progressionType;

                // Load the next scene
                LoadSceneForProgression(currentDay, progressionType, isDayStart);
                return false;
            }
        }

        return true; // All parts for the day are completed
    }

    private void LoadSceneForProgression(int currentDay, ProgressionType progressionType, bool isDayStart)
    {
        CrossSceneMessage.Send(currentDay.ToString(), progressionType);

        switch (progressionType)
        {
            case ProgressionType.CutScene:
                GameSceneManager.Instance.LoadCutsceneScene(isDayStart);
                break;
            case ProgressionType.EarlyVisualNovel:
            case ProgressionType.MiddleVisualNovel:
            case ProgressionType.EndVisualNovel:
                GameSceneManager.Instance.LoadVisualNovelScene(isDayStart);
                break;
            case ProgressionType.Shop:
                GameSceneManager.Instance.LoadShopScene(isDayStart);
                break;
            case ProgressionType.Gathering:
                GameSceneManager.Instance.LoadGatheringScene(isDayStart);
                break;
            case ProgressionType.Credit:
                GameSceneManager.Instance.LoadCreditScene(isDayStart);
                break;
            default:
                break;
        }
    }

    public void SetNullProgressionType()
    {
        lastCompletedProgressionType = null;
    }
}
