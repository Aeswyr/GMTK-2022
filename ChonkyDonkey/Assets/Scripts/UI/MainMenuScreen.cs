using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    public MainMenuScreen menuScreen;
    public OptionsScreen optionsScreen;
    public CreditsScreen creditsScreen;

    public void OnPlayClicked()
    {
        FlipCupGameStats.ResetGameStats();
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine() 
    {
        SFXHelper.PlaySound(SFXHelper.mainMenuSqueakyName);

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
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
