using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public MainMenuScreen menuScreen;
    public OptionsScreen optionsScreen;
    public CreditsScreen creditsScreen;

    public void OnPlayClicked() 
    {
        // TODO load the gameplay screen
    }

    public void OnOptionsClicked() 
    {
        optionsScreen.gameObject.SetActive(true);
    }

    public void OnCreditsClicked()
    {
        creditsScreen.gameObject.SetActive(true);
    }
}
