using UnityEngine;

public class Drunkeness : Singleton<Drunkeness>
{

    public int currentIndex = 0;
    [SerializeField] private int peakIndex = 3;
    [SerializeField] private int maxIndex = 9;

    [SerializeField] private int rollScale = 1;

    private int[] rollModifiers;

    private readonly Color neutralColor = Color.yellow; 
    private readonly Color buzzedColor = Color.green; 
    private readonly Color wastedColor = Color.red;

    public delegate void Notify(int delta);
    public event Notify DrunkenessChanged;

    void Awake()
    {
        var self = Instance; // initialize singleton on awake
        rollModifiers = CreateDrunkenessArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConsumeDrink(int drinkValue)
    {
        if(currentIndex < maxIndex)
        {
            currentIndex += drinkValue;
            DrunkenessChanged?.Invoke(drinkValue);
        }
    }

    public void ReduceDrunkeness(int value)
    {
        if (currentIndex > 0)
        {
            currentIndex -= value;
            DrunkenessChanged?.Invoke(-value);
        }
    }

    public int GetRollModifier()
    {
        return rollModifiers[currentIndex] * rollScale;
    }

    public float GetDrunkenessPercentage()
    {
        return currentIndex / (float) maxIndex;
    }

    public Color GetIntoxicationColor()
    {
        int rollModifier = GetRollModifier();
        
        if(rollModifier > 0)
        {
            return buzzedColor;
        }
        else if(rollModifier < 0)
        {
            return wastedColor;
        }

        return neutralColor;
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
