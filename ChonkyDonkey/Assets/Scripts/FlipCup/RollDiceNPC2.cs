using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceNPC2 : MonoBehaviour
{
    public static Vector3 velocity;
    public static int roll = 0;

    private static Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("rollDice", 0, 7);
    }

    public static void drinkRandomCup(string name)
    {
        setFlag(name, false);
        GameObject allCups = GameObject.Find("Cups");
        int random = Random.Range(0, allCups.transform.childCount);
        FlipCupGameStats.NPC2SixCount += 1;
        FlipCupHUD.Instance.OnDrinksChange(FlipCupGameStats.playerSixCount, FlipCupGameStats.NPC1SixCount + FlipCupGameStats.NPC2SixCount);

        Destroy(allCups.transform.GetChild(random).gameObject);
    }

    private static void setFlag(string name, bool status)
    {
        switch(name)
        {
            case "NPCDice1":
                FlipCupGameStats.rolledNPC1Dice = status;
                break;
            case "NPCDice2":
                FlipCupGameStats.rolledNPC2Dice = status;
                break;
        }
    }
    private void rollDice()
    {
        setFlag(gameObject.name, true);
        float dirX = Random.Range(50, 200);
        float dirY = Random.Range(50, 200);
        float dirZ = Random.Range(50, 200);
        transform.position = transform.parent.position + new Vector3(2.5f, 87f, 10f) + new Vector3(0, 5f, 0);
        transform.rotation = new Quaternion(-29f, 45f, -110f, 0f);
        rb.AddForce(transform.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
