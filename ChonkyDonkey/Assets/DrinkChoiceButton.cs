using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrinkChoiceButton : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI CountLabel;
    public Animator Controller;
    private static readonly int Drink = Animator.StringToHash("Drink");

    public void SetDrinkSize(int size)
    {
        CountLabel.text = $"{size}/5";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("On Cup Clicked: " + name + "size: " + CountLabel.text);
        Controller.SetTrigger(Drink);
    }
}
