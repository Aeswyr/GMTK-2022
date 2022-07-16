using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle soundToggle;

    public void OnAwake()
    {
        // TODO sync these values to some global settings object
        volumeSlider.SetValueWithoutNotify(0.5f);
        soundToggle.SetIsOnWithoutNotify(true);
    }

    public void OnBackButtonClicked() 
    {
        gameObject.SetActive(false);
    }

    public void OnSliderChanged() 
    {
        Debug.Log($"OnSliderChanged: {volumeSlider.value}");
    }
    
    public void OnSoundToggleChanged(bool value)
    {
        Debug.Log($"OnSoundToggleChanged: {value}");
    }
}
