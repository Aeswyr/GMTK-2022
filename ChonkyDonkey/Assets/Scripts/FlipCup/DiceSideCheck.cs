using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideCheck : MonoBehaviour
{
    public static int playerRoll = 0;
    public static int npc1Roll = 0;
    public static int npc2Roll = 0;

    public GameObject affinityBar;

    void FixedUpdate()
    {
        if (!FlipCupGameStats.canDrink)
        {
            enableCollider(true);
        }
    }

    public int GetPlayerRoll()
    {
        return playerRoll;
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
                case "Side2":
                    playerRoll = 5;
                    break;
                case "Side3":
                    playerRoll = 4;
                    break;
                case "Side4":
                    playerRoll = 3;
                    break;
                case "Side5":
                    playerRoll = 2;
                    break;
                case "Side6":
                    playerRoll = 1;
                    break;
            }
            if (ModeManager.Instance.Mode == GameMode.AwooDice && AffinityBar.rolledDice)
            {
                AffinityBar bar = affinityBar.GetComponent<AffinityBar>();
                bar.UpdateAffinityAfterRoll(bar.GetDogTag());

            }
            if (name == "Side1")
            {
                if (FlipCupGameStats.rolledPlayerDice && playerRoll == 6)
                {
                    FlipCupGameStats.canDrink = true;
                    enableCollider(false);
                    DrinkChoiceOverlay.Instance.Show(FindObjectOfType<CupStorage>());
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
