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
            EndGame();
        }
    }

    public void EndGame()
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
        
        if (FlipCupGameStats.diceCount < 0 && ModeManager.Instance.Mode == GameMode.AwooDice)
        {
            // END THE GAME LOSE CONDITION
            EndingScreenUI.Instance.Show(EndingScreenUI.EndingResult.KickedOut, PetId.Default);
        }
        
        ModeManager.Instance.ChangeMode(GameMode.Bar);
    }

    public bool endGameState()
    {
        return (gameObject.transform.childCount == 0 || FlipCupGameStats.diceCount < 0);
    }

    public int[] GetCupList()
    {
        List<int> cupData = new List<int>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cup = transform.GetChild(i).GetComponent<CupHandler>();
            if (cup != null)
            {
                cupData.Add(cup.thirst);
            }
        }
        return cupData.ToArray();
    }

    public void RemoveCup(int thirstValue)
    {
        // only remove first
        for (int i = 0; i < transform.childCount; i++)
        {
            var cup = transform.GetChild(i).GetComponent<CupHandler>();
            if (cup != null)
            {
                if (cup.thirst == thirstValue)
                {
                    cup.OnDrink();
                    return;
                }
            }
        }
        // if we got here, we failed, warn
        Debug.LogError("Drank a cup, but the cup was not found!");
    }

    public void restoreCups()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        float topSlot = -5;
        for (int i = 1; i <= 5; i++)
        {
            int thirstVal = i;
            string name = "Cup" + i.ToString();
            GameObject cup = Instantiate(cupPrefab, gameObject.transform.position + Vector3.forward * .1f * (i-1), Quaternion.Euler(-11.96f,0,0));
            Debug.Log("cup created " + cup + " thirst:" + cup.GetComponent<CupHandler>().thirst);
            cup.GetComponent<CupHandler>().thirst = thirstVal;
            Debug.Log("thirst val " + thirstVal);
            Debug.Log("cup " + cup + " thirst:" + cup.GetComponent<CupHandler>().thirst);
            cup.transform.parent = gameObject.transform;
            cup.transform.position = new Vector3(cup.transform.position.x + 10f, cup.transform.position.y + 2.5f, cup.transform.position.z + topSlot);
            cup.gameObject.name = name;
            topSlot += 2.5f;
        }
    }
}
