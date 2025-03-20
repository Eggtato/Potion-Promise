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

    public void PlayClickSound()
    {
        PlaySFX(gameSoundSO.ClickSound);
    }

    public void PlayTypeSound()
    {
        PlaySFX(gameSoundSO.TypeSound);
    }

    private void PlaySFX(AudioClip audio)
    {
        PlaySound(audio, MMSoundManager.MMSoundManagerTracks.Sfx);
    }

    private void PlayMusic(AudioClip audio)
    {
        PlaySound(audio, MMSoundManager.MMSoundManagerTracks.Music);
    }

    private void PlaySound(AudioClip audio, MMSoundManager.MMSoundManagerTracks track)
    {
        MMSoundManagerSoundPlayEvent.Trigger(audio, track, transform.position);
    }
}