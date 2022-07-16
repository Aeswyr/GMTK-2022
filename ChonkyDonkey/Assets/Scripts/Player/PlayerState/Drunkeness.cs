using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunkeness : MonoBehaviour
{

    public int currentIndex = 0;
    [SerializeField] private int peakIndex = 3;
    [SerializeField] private int maxIndex = 9;

    [SerializeField] private int rollScale = 1;

    private int[] rollModifiers; 

    // Start is called before the first frame update
    void Start()
    {
        rollModifiers = CreateDrunkenessArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConsumeDrink(int drinkValue)
    {
        currentIndex += drinkValue;
    }

    public void ReduceDrunkeness(int value)
    {
        currentIndex -= value;
    }

    public int GetRollModifier()
    {
        return rollModifiers[currentIndex] * rollScale;
    }

    public float GetDrunkenessPercentage()
    {
        return currentIndex / maxIndex;
    }

    private int[] CreateDrunkenessArray()
    {
        int[] arr = new int[maxIndex + 1];

        int rollModifier = 0;
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = rollModifier;

            if(i < peakIndex)
            {
                rollModifier++;
            }
            else
            {
                rollModifier--;
            }
        }

        return arr;
    }
}
