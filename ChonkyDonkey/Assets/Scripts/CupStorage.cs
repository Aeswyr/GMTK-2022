using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStorage : MonoBehaviour
{
    private bool done;
    void Update()
    {
        if (!done && Time.timeScale > 0 && (gameObject.transform.childCount == 0 || FlipCupGameStats.diceCount <= 0))
        {
            done = true;
            Debug.Log("Ending game!");
            if (FlipCupGameStats.checkWinCondition())
            {
                FlipCupGameStats.rewardDice();
            }
            Debug.Log("Thirst Level Gained: " + FlipCupGameStats.thirst);
            Debug.Log("CupDice Spent: " + FlipCupGameStats.spentDice);
            Debug.Log("CupDice Count: " + FlipCupGameStats.diceCount);
            Time.timeScale = 1f;
            ModeManager.Instance.ChangeMode(GameMode.Bar);
        }
    }
}
