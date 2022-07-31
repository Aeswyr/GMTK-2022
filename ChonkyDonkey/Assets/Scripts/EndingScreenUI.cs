using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScreenUI : Singleton<EndingScreenUI>
{
    public enum EndingResult
    {
        KickedOut,
        AwooOne,
        AwooAll
    }
    
    public Image BackgroundImage;
    public Image PlayerCharacterIcon;
    public Image AwooCharacterIcon;
    public Image SadHaku;
    public TextMeshProUGUI NameLabel;
    public TypewriterText Typewriter;
    public CanvasGroup OpacityController;
    public GameObject DiceCounter;
    //public Animator Controller;

    [Header("Assets")] 
    public Sprite LoseBackground;
    public Sprite WinBackground;
    public Sprite PerfectWinBackground;
    
    [Header("Config")] 
    public float TypewriterSpeed;
    public float TypewriterDelay;
    
    
    // who did the player take home
    // null if everyone or no one
    private PetId chosenPartnerDog;
    

    public void Show(EndingResult result, PetId chosen)
    {
        chosenPartnerDog = chosen;
        
        // show
        OpacityController.alpha = 1;
        OpacityController.interactable = true;
        OpacityController.blocksRaycasts = true;

        // collect status
        Sprite playerSprite = null;
        Sprite partnerSprite = null;
        Sprite background = null;
        string speakerName = "Narrator";
        string endingText = "The End";
        switch (result)
        {
            case EndingResult.KickedOut:
                partnerSprite = null;
                playerSprite = CharacterSprites.Load(PetId.Haku).Angry;
                background = LoseBackground;
                endingText = "No dice! Try your luck next time.";
                break;
            case EndingResult.AwooAll:
                partnerSprite = null;
                playerSprite = null;
                background = PerfectWinBackground;
                endingText = "Amazing! You've \"awoooo'd\" everyone! Thank you for playing!";
                break;
            case EndingResult.AwooOne:
                partnerSprite = CharacterSprites.Load(chosenPartnerDog).Happy;
                playerSprite = CharacterSprites.Load(PetId.Haku).Happy;
                background = WinBackground;
                endingText = $"{StatsLoader.Get(PetId.Haku).DisplayName} and {StatsLoader.Get(chosenPartnerDog).DisplayName} leave the bar together... Congratulations! Thank you for playing!";
                break;
        }

        // update ui
        NameLabel.text = speakerName;
        Typewriter.PlayTypewriter(endingText, TypewriterSpeed, delay: TypewriterDelay);
        AwooCharacterIcon.enabled = partnerSprite != null;
        AwooCharacterIcon.sprite = partnerSprite;
        PlayerCharacterIcon.enabled = result == EndingResult.AwooOne;
        PlayerCharacterIcon.sprite = playerSprite;
        BackgroundImage.sprite = background;
        SadHaku.enabled = result == EndingResult.KickedOut;
        DiceCounter.SetActive(result == EndingResult.KickedOut);
    }

    // attached in inspector
    public void OnMenuPressed()
    {
        SceneManager.LoadScene("MainMenuScreen");
    }
}
