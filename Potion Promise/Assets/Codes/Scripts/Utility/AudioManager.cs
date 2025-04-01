using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;
using UnityEngine.Rendering;
using MoreMountains.Tools;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private GameSoundSO gameSoundSO;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player sfxFeedbacks;
    [SerializeField] private MMF_Player musicFeedbacks;
    [SerializeField] private MMF_Player coinConvertFeedbacks;

    public void PlayClickSound()
    {
        PlaySFX(gameSoundSO.ClickSound);
    }

    public void PlayTypeSound()
    {
        PlaySFX(gameSoundSO.TypeSound);
    }

    public void PlayDayEndSound()
    {
        PlaySFX(gameSoundSO.DayEndSound);
    }

    public void PlayCoinSound()
    {
        PlayShortSFX(gameSoundSO.CoinSound);
    }

    private void PlayMusic(AudioClip audio)
    {
        // PlaySound(audio, MMSoundManager.MMSoundManagerTracks.Music);
        MMF_MMSoundManagerSound sound = musicFeedbacks.GetFeedbackOfType<MMF_MMSoundManagerSound>();
        sound.Sfx = audio;
        musicFeedbacks.PlayFeedbacks();
    }

    private void PlaySFX(AudioClip audio)
    {
        // PlaySound(audio, MMSoundManager.MMSoundManagerTracks.Sfx);
        MMF_MMSoundManagerSound sound = sfxFeedbacks.GetFeedbackOfType<MMF_MMSoundManagerSound>();
        sound.Sfx = audio;
        sfxFeedbacks.PlayFeedbacks();
    }

    private void PlayShortSFX(AudioClip audio)
    {
        MMF_MMSoundManagerSound sound = coinConvertFeedbacks.GetFeedbackOfType<MMF_MMSoundManagerSound>();
        sound.Sfx = audio;
        coinConvertFeedbacks.PlayFeedbacks();
    }

    private void PlaySound(AudioClip audio, MMSoundManager.MMSoundManagerTracks track)
    {
        MMSoundManagerSoundPlayEvent.Trigger(audio, track, transform.position);
    }
}