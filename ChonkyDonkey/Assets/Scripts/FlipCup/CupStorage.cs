using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStorage : MonoBehaviour
{
    public GameObject flipCupGameHandler;
    public GameObject cupPrefab;

    void Update()
    {
        if (endGameState())
        {
            Debug.Log("Ending game!");
            if (FlipCupGameStats.checkWinCondition())
            {
                FlipCupGameStats.rewardDice();
            }
            Debug.Log("Thirst Level Gained: " + FlipCupGameStats.thirst);
            Debug.Log("Dice Spent: " + FlipCupGameStats.spentDice);
            Debug.Log("Dice Count: " + FlipCupGameStats.diceCount);
            restoreCups();
            FlipCupGameHandler handler = flipCupGameHandler.GetComponent<FlipCupGameHandler>();
            if (!handler.MainCamera.activeSelf)
            {
                handler.ToggleFlipCup();
            }

            if (FlipCupGameStats.diceCount < 0)
            {
                Time.timeScale = 0f;
            }
        }
    }

    public bool endGameState()
    {
        return (gameObject.transform.childCount == 0 || FlipCupGameStats.diceCount < 0);
    }

    private void restoreCups()
    {
        float topSlot = -5;
        for (int i = 1; i <= 5; i++)
        {
            string name = "Cup" + i.ToString();
            GameObject cup = Instantiate(cupPrefab, gameObject.transform.position, Quaternion.identity);
            cup.gameObject.GetComponent<CupHandler>().thirst = i;
            cup.transform.parent = gameObject.transform;
            cup.transform.position = new Vector3(cup.transform.position.x + 10f, cup.transform.position.y + 2.5f, cup.transform.position.z + topSlot);
            cup.gameObject.name = name;
            topSlot += 2.5f;
        }
    }
}
