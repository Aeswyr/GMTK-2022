using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CupHandler : MonoBehaviour, IPointerClickHandler
{
    public int thirst = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Replaced with UI, see DrinkChoiceButton
    }

    public void OnDrink()
    {
        Debug.Log(thirst + " Cup drank!");
        
        FlipCupHUD.Instance.SetInstructions("Cup drank with alcohol content of: " + thirst);
        FlipCupHUD.Instance.OnDrinksChange(FlipCupGameStats.playerSixCount+1, FlipCupGameStats.NPC1SixCount + FlipCupGameStats.NPC2SixCount);
    
        Drunkeness.Instance.ConsumeDrink(thirst);
        FlipCupGameStats.thirst += thirst;
        FlipCupGameStats.playerSixCount += 1;
        FlipCupGameStats.canDrink = false;
        FlipCupGameStats.rolledPlayerDice = false;
        Destroy(gameObject);
    }
}
