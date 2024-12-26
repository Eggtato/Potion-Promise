using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private MMFeedbacks uiClickFeedbacks;
    [SerializeField] private MMFeedbacks typeSoundFeedbacks;

    public void PlayClickFeedbacks()
    {
        uiClickFeedbacks.PlayFeedbacks();
    }

    public void PlayTypeFeedbacks()
    {
        typeSoundFeedbacks.PlayFeedbacks();
    }
}