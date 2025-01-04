using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingTabUI : SettingTabUI
{
    [Header("Texts")]
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text bgmVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        OnMasterSliderValueChanged(masterSlider.value);
        OnBgmSliderValueChanged(bgmSlider.value);
        OnSfxSliderValueChanged(sfxSlider.value);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        masterSlider.onValueChanged.AddListener(OnMasterSliderValueChanged);
        bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        masterSlider.onValueChanged.RemoveListener(OnMasterSliderValueChanged);
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderValueChanged);
    }

    private void OnMasterSliderValueChanged(float volume)
    {
        masterVolumeText.text = $"{Mathf.RoundToInt(volume / masterSlider.maxValue * 100)}";
    }

    private void OnBgmSliderValueChanged(float volume)
    {
        bgmVolumeText.text = $"{Mathf.RoundToInt(volume / bgmSlider.maxValue * 100)}";
    }

    private void OnSfxSliderValueChanged(float volume)
    {
        sfxVolumeText.text = $"{Mathf.RoundToInt(volume / sfxSlider.maxValue * 100)}";
    }
}
