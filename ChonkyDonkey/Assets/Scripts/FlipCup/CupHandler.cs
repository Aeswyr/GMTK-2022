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
    
        TMPro.TextMeshProUGUI tmText = GameObject.FindWithTag("FlipCupHUD").GetComponent<TMPro.TextMeshProUGUI>();
        tmText.text = "Cup drank with alcohol content of: " + thirst;
    
        Drunkeness.Instance.ConsumeDrink(thirst);
        FlipCupGameStats.thirst += thirst;
        FlipCupGameStats.playerSixCount += 1;
        FlipCupGameStats.canDrink = false;
        FlipCupGameStats.rolledPlayerDice = false;
        Destroy(gameObject);
    }
}
