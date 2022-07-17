using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle soundToggle;

    public void Awake()
    {
        volumeSlider.SetValueWithoutNotify(SFXHelper.Instance.Volume);
        soundToggle.SetIsOnWithoutNotify(SFXHelper.Instance.SoundEnabled);

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
        SFXHelper.Instance.Volume = volumeSlider.value;
    }

    void OnSoundToggleChanged(Toggle change)
    {
        SFXHelper.Instance.SoundEnabled = soundToggle.isOn;
    }
}
