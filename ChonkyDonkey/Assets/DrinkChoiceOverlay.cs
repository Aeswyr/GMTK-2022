using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DrinkChoiceOverlay : Singleton<DrinkChoiceOverlay>
{
    public DrinkChoiceButton[] DrinkButtons;
    private CupStorage cupStorage;
    private static readonly int Reject = Animator.StringToHash("Reject");

    public void Show(CupStorage cupStorage)
    {
        this.cupStorage = cupStorage;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        var drinks = cupStorage.GetCupList();
        for (int i = 0; i < DrinkButtons.Length; i++)
        {
            if (i >= drinks.Length)
            {
                DrinkButtons[i].gameObject.SetActive(false);
            }
            else
            {
                DrinkButtons[i].gameObject.SetActive(true);
                DrinkButtons[i].Initialize(i,drinks[i], OnDrinkChosen, OnDrinkDrunk);
            }
        }
    }

    private void OnDrinkChosen(DrinkChoiceButton button)
    {
        for (int i = 0; i < DrinkButtons.Length; i++)
        {
            if (DrinkButtons[i] == button)
            {
                // handled by button
            }
            else
            {
                DrinkButtons[i].Controller.SetTrigger(Reject);
            }
        }
    }

    private void OnDrinkDrunk(DrinkChoiceButton button)
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        cupStorage.RemoveCup(button.DrinkSize);
        gameObject.SetActive(false);
    }
}
