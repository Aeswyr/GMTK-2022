using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle soundToggle;
    public AudioSource backgroundMusic;

    private float initialSFXVolume;
    private float initialBackgroundVolume;

    public void Awake()
    {
        initialSFXVolume = SFXHelper.Instance.Volume;
        initialBackgroundVolume = backgroundMusic.volume;

        volumeSlider.SetValueWithoutNotify((initialBackgroundVolume * volumeSlider.value)/initialBackgroundVolume);
        soundToggle.SetIsOnWithoutNotify(SFXHelper.Instance.SoundEnabled);

        OnSliderChanged();

        soundToggle.onValueChanged.AddListener(delegate {
            OnSoundToggleChanged(soundToggle);

        });
    }

    public void OnBackButtonClicked() 
    {
        gameObject.SetActive(false);
    }

    public void OnSliderChanged() 
    {
        SFXHelper.Instance.Volume = initialSFXVolume * volumeSlider.value;
        backgroundMusic.volume = initialBackgroundVolume * volumeSlider.value;
    }

    void OnSoundToggleChanged(Toggle change)
    {
        SFXHelper.Instance.SoundEnabled = soundToggle.isOn;
        backgroundMusic.enabled = soundToggle.isOn;
    }
}
