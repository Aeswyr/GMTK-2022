using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUI : MonoBehaviour
{

    [SerializeField] DiceInventory diceInventory;
    [SerializeField] TMPro.TextMeshProUGUI diceAmountLabel;


    // Start is called before the first frame update
    void Start()
    {
        diceAmountLabel.text = diceInventory.currentNumberOfDice.ToString();
    }

    private int prevDice = 0;

    // Update is called once per frame
    void Update()
    {
        if (FlipCupGameStats.diceCount != prevDice)
        {
            diceInventory.currentNumberOfDice = FlipCupGameStats.diceCount;
            onDiceAmountChanged();
        }

    }

    public void onDiceAmountChanged()
    {
        diceAmountLabel.text = diceInventory.currentNumberOfDice.ToString();
    }
}