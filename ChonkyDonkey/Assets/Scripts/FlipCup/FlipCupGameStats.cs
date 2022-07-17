using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCupGameStats : MonoBehaviour
{
    public static int diceCount = 20;
    public static int thirst = 0;
    public static int spentDice = 0;

    public static bool canDrink = false;
    public static bool rolledPlayerDice = false;
    public static bool rolledNPC1Dice = false;
    public static bool rolledNPC2Dice = false;

    public static int playerSixCount = 0;
    public static int NPC1SixCount = 0;
    public static int NPC2SixCount = 0;

    public static void spendDice()
    {
        FlipCupGameStats.diceCount -= 1;
        FlipCupGameStats.spentDice += 1;
    }

    public static bool checkWinCondition()
    {
        return ((playerSixCount > NPC1SixCount) && (playerSixCount > NPC2SixCount));
    }

    public static void rewardDice()
    {
        diceCount += (spentDice + (spentDice / 2));
    }
}
