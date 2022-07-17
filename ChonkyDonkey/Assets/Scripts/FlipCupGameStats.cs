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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Thirst Level: " + thirst);
        }
    }
}
