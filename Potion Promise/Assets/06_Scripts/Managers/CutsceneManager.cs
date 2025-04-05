using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [Serializable]
    public class CutsceneConfig
    {
        public int Day;
        public Transform CutsceneTransform;
    }

    [Header("Project Reference")]
    [SerializeField] private PlayerEventSO playerEventSO;
    [SerializeField] private List<CutsceneConfig> cutsceneConfigs = new();

    private Dictionary<int, CutsceneConfig> cutsceneDictionary;
    private VideoPlayer currentVideoPlayer;

    private void Awake()
    {
        // Convert list to dictionary for O(1) lookup
        cutsceneDictionary = new Dictionary<int, CutsceneConfig>();
        foreach (var config in cutsceneConfigs)
        {
            if (config != null)
                cutsceneDictionary[config.Day] = config;
        }
    }

    private void Start()
    {
        int currentDay = GameDataManager.Instance.CurrentDay;

        // Check if the cutscene exists for the current day
        if (cutsceneDictionary.TryGetValue(currentDay, out var cutscene))
        {
            if (cutscene.CutsceneTransform != null)
            {
                cutscene.CutsceneTransform.gameObject.SetActive(true);
                currentVideoPlayer = cutscene.CutsceneTransform.GetComponentInChildren<VideoPlayer>();

                if (currentVideoPlayer != null)
                    currentVideoPlayer.loopPointReached += HandleVideoFinishPlaying;
                else
                    Debug.LogWarning($"No VideoPlayer found in {cutscene.CutsceneTransform.name}");
            }
        }

        // Disable all other cutscenes
        foreach (var kvp in cutsceneDictionary)
        {
            if (kvp.Key != currentDay && kvp.Value.CutsceneTransform != null)
            {
                kvp.Value.CutsceneTransform.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        if (currentVideoPlayer != null)
        {
            currentVideoPlayer.loopPointReached -= HandleVideoFinishPlaying;
        }
    }

    private void HandleVideoFinishPlaying(VideoPlayer source)
    {
        playerEventSO.Event.OnGoToNextScene();
    }
}
