using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlipCupHUD : Singleton<FlipCupHUD>
{
    [Header("Children")]
    public TextMeshProUGUI InstructionsLabel;
    public TextMeshProUGUI YourDiceLabel;
    public TextMeshProUGUI WinnerRewardLabel;
    public Image PlayerCharacterIcon;
    public Image OpponentCharacterIcon;
    public GameObject[] PlayerDrinksIcons;
    public GameObject[] OpponentDrinksIcons;
    public GameObject PlayerDiceLowWarningIcon;
    public GameObject LastDrinkWarningIcon;

    public void Awake()
    {
        instance = this; // init singleton instance, even if disabled
    }

    public void OnGameStart()
    {
        PetId player = FlipCupGameStats.playerId, opponent = FlipCupGameStats.opponentId; 
        int playerDice = FlipCupGameStats.diceCount;
        
        // set characters
        PlayerCharacterIcon.sprite = CharacterSprites.Load(player).Default;
        OpponentCharacterIcon.sprite = CharacterSprites.Load(opponent).Default;
        // reset dice
        OnPlayerDiceChange(playerDice);
        // reset drinks
        OnDrinksChange(0, 0);
        // reward
        OnRewardChange(1); // default reward
        // instructions
        SetInstructions("Click the white dice to begin.\n\nWhen you roll a six, click on a cup to drink! Try and roll the most sixes before your two opponents! The player who drinks the most cups will win more dice!");
    }

    public void SetInstructions(string instructions)
    {
        InstructionsLabel.text = instructions;
    }

    private static void Populate(GameObject[] icons, int count)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].SetActiveFast(i < count);
        }
    }

    public void OnDrinksChange(int playerDrinks, int opponentDrinks)
    {
        Debug.Log("OnDrinksChange |" + playerDrinks + "|" + opponentDrinks);
        Populate(PlayerDrinksIcons, playerDrinks);
        Populate(OpponentDrinksIcons, opponentDrinks);
        LastDrinkWarningIcon.SetActiveFast(playerDrinks + opponentDrinks > 3);
    }

    public void OnPlayerDiceChange(int newDice)
    {
        YourDiceLabel.text = "x " + newDice;
        PlayerDiceLowWarningIcon.SetActiveFast(newDice <= 5);
    }

    public void OnRewardChange(int newReward)
    {
        WinnerRewardLabel.text = "x " + Mathf.Max(1,newReward);
    }
}
