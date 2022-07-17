using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupHandler : MonoBehaviour
{
    public int thirst = 0;

    private void OnMouseDown()
    {
        if (FlipCupGameStats.canDrink && FlipCupGameStats.rolledPlayerDice)
        {
            Debug.Log(thirst + " Cup drank!");
            FlipCupGameStats.thirst += thirst;
            FlipCupGameStats.canDrink = false;
            FlipCupGameStats.rolledPlayerDice = false;
            RollDice.changeColor(Color.blue);
            Destroy(gameObject);
        }
    }
}
