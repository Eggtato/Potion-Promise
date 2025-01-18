using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private MMFeedbacks uiClickFeedbacks;
    [SerializeField] private MMFeedbacks typeSoundFeedbacks;

    public void PlayClickSound()
    {
        uiClickFeedbacks.PlayFeedbacks();
    }

    public void PlayTypeSound()
    {
        typeSoundFeedbacks.PlayFeedbacks();
    }
}