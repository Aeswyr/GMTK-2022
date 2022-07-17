using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStorage : MonoBehaviour
{

    void Update()
    {
        if (Time.timeScale > 0 && (gameObject.transform.childCount == 0 || FlipCupGameStats.diceCount <= 0))
        {
            Debug.Log("Ending game!");
            if (FlipCupGameStats.checkWinCondition())
            {
                FlipCupGameStats.rewardDice();
            }
            Debug.Log("Thirst Level Gained: " + FlipCupGameStats.thirst);
            Debug.Log("Dice Spent: " + FlipCupGameStats.spentDice);
            Debug.Log("Dice Count: " + FlipCupGameStats.diceCount);
            Time.timeScale = 0f;
        }
    }
}
