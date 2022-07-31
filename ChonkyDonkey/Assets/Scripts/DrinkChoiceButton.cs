using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrinkChoiceButton : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI CountLabel;
    public Animator Controller;
    public int DrinkSize => drinkSize;
    
    private static readonly int Drink = Animator.StringToHash("Drink");
    private static readonly int Reset = Animator.StringToHash("Reset");
    private bool isAnimating;
    private Action<DrinkChoiceButton> beginDrinkCallback;
    private Action<DrinkChoiceButton> didDrinkCallback;
    private int index;
    private int drinkSize;

    public void Initialize(int index, int drinkSize, Action<DrinkChoiceButton> chosenCallback, Action<DrinkChoiceButton> doneCallback)
    {
        this.index = index;
        this.drinkSize = drinkSize;
        beginDrinkCallback = chosenCallback;
        didDrinkCallback = doneCallback;
        Controller.SetTrigger(Reset);
        
        // update ui
        CountLabel.text = $"{drinkSize}/5";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("On Cup Clicked: " + name + "size: " + CountLabel.text);
        if (isAnimating) return;
        isAnimating = true;
        Debug.Log("Begin Cup Anim: " + name + "size: " + CountLabel.text);
        Controller.SetTrigger(Drink);
        beginDrinkCallback.Invoke(this);
    }

    // called by Animator key event
    // ReSharper disable once UnusedMember.Global
    public void OnAnimationComplete()
    {
        didDrinkCallback.Invoke(this);
        gameObject.SetActive(false);
    }
}
