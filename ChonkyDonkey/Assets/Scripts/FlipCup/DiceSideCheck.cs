using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideCheck : MonoBehaviour
{
    public static int playerRoll = 0;
    public static int npc1Roll = 0;
    public static int npc2Roll = 0;

    void FixedUpdate()
    {
        if (!FlipCupGameStats.canDrink)
        {
            enableCollider(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        playerRoll = 0;
        npc1Roll = 0;
        npc2Roll = 0;
        Rigidbody rb = col.gameObject.transform.parent.GetComponent<Rigidbody>();
        if (rb.velocity.x == 0f && rb.velocity.y == 0f && rb.velocity.z == 0f) {
            string name = col.gameObject.name;
            switch (name) {
                case "Side1":
                    playerRoll = 6;
                    break;
                case "NPC1Side1":
                    npc1Roll = 6;
                    break;
                case "NPC2Side1":
                    npc2Roll = 6;
                    break;
            }
            if (name == "Side1")
            {
                if (FlipCupGameStats.rolledPlayerDice && playerRoll == 6)
                {
                    FlipCupGameStats.canDrink = true;
                    enableCollider(false);
                }
            }
            else
            {
                if (name == "NPC1Side1" && FlipCupGameStats.rolledNPC1Dice && npc1Roll == 6)
                {
                    RollDiceNPC.drinkRandomCup(col.gameObject.transform.parent.name);
                }
                if (name == "NPC2Side1" && FlipCupGameStats.rolledNPC2Dice && npc2Roll == 6)
                {
                    RollDiceNPC2.drinkRandomCup(col.gameObject.transform.parent.name);
                }
            }
        }
    }

    private void enableCollider(bool status)
    {
        GetComponent<BoxCollider>().enabled = status;
    }
}
