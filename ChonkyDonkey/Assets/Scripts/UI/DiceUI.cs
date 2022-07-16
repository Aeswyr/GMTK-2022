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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onDiceAmountChanged()
    {
        diceAmountLabel.text = diceInventory.currentNumberOfDice.ToString();
    }
}
