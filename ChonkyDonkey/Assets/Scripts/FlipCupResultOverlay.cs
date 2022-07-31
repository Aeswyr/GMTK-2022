using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class FlipCupResultOverlay : Singleton<FlipCupResultOverlay>
{
    public CanvasGroup CanvasGroup;
    public Animator Controller;
    public Image WinnerCharacterIcon;
    public Image PlayerCharacterIcon;
    public Image OpponentCharacterIcon;
    public GameObject[] PlayerScoreObjects;
    public GameObject[] OpponentScoreObjects;
    public GameObject YouWonBanner;
    public TextMeshProUGUI ResultHeader;
    public TextMeshProUGUI ResultAmountLabel;
    public bool IsShowing;

    private float showTime;
    
    // animator hashes
    private static readonly int Victory = Animator.StringToHash("Victory");
    private static readonly int Defeat = Animator.StringToHash("Defeat");
    private static readonly int Reset = Animator.StringToHash("Reset");

    private void Awake()
    {
        instance = this;
    }

    public void Show()
    {
        IsShowing = true;
        showTime = Time.time;
        int score = FlipCupGameStats.playerSixCount;
        int opponentScore = FlipCupGameStats.NPC1SixCount + FlipCupGameStats.NPC2SixCount;
        bool playerWon = score > opponentScore;

        // animation
        Controller.SetTrigger( playerWon ? Victory : Defeat);
        
        // special banner for player
        YouWonBanner.SetActive(playerWon);
        
        // icons
        PlayerCharacterIcon.sprite = CharacterSprites.Load(FlipCupGameStats.playerId).Get(playerWon ? DogReactionType.Happy : DogReactionType.Angry);
        OpponentCharacterIcon.sprite = CharacterSprites.Load(FlipCupGameStats.opponentId).Get(playerWon ? DogReactionType.Angry : DogReactionType.Happy);
        WinnerCharacterIcon.sprite = CharacterSprites.Load(playerWon ? FlipCupGameStats.playerId : FlipCupGameStats.opponentId).Happy;
        
        // scores
        Populate(PlayerScoreObjects, score);
        Populate(OpponentScoreObjects, opponentScore);
        
        // rewards
        ResultHeader.text = playerWon ? "You got:" : "You lost:";
        ResultAmountLabel.text = "" + (playerWon ? FlipCupGameStats.GetWinAmount() : FlipCupGameStats.spentDice);
        
        // prepare for interaction
        SetInteractable(true);
    }

    // Attached in inspector
    // ReSharper disable once UnusedMember.Global
    public void OnClosePressed()
    {
        if (Time.time - showTime < 0.5f) return; // prevent accidental tap
        SetInteractable(false);
        Controller.SetTrigger(Reset);
        IsShowing = false;
        
        if (FlipCupGameStats.diceCount < 0)
        {
            // END THE GAME LOSE CONDITION
            EndingScreenUI.Instance.Show(EndingScreenUI.EndingResult.KickedOut, PetId.Default);
        }
    }

    private void SetInteractable(bool interactable)
    {
        CanvasGroup.interactable = interactable;
        CanvasGroup.blocksRaycasts = interactable;
    }
    
    private static void Populate(GameObject[] icons, int count)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].SetActiveFast(i < count);
        }
    }
}
