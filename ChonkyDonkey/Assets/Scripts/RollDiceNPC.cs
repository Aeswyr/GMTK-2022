using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceNPC : MonoBehaviour
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
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.position = transform.parent.position + new Vector3(0, 2f, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
