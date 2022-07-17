using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CupHandler : MonoBehaviour, IPointerClickHandler
{
    public int thirst = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FlipCupGameStats.canDrink && FlipCupGameStats.rolledPlayerDice)
        {
            Debug.Log(thirst + " Cup drank!");

            TMPro.TextMeshProUGUI tmText = GameObject.FindWithTag("FlipCupHUD").GetComponent<TMPro.TextMeshProUGUI>();
            tmText.text = "Cup drank with alcohol content of: " + thirst;

            FlipCupGameStats.thirst += thirst;
            FlipCupGameStats.playerSixCount += 1;
            FlipCupGameStats.canDrink = false;
            FlipCupGameStats.rolledPlayerDice = false;
            Destroy(gameObject);
        }
    }
}
