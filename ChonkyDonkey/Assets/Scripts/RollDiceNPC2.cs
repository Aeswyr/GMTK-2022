using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceNPC2 : MonoBehaviour
{
    public static Vector3 velocity;
    public static int roll = 0;

    private static Rigidbody rb;
    private static MeshRenderer diceRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        diceRenderer = GetComponent<MeshRenderer>();

        InvokeRepeating("rollDice", 0, 7);
    }

    public static void drinkRandomCup(string name)
    {
        setFlag(name, false);
        changeColor(Color.red);
        GameObject allCups = GameObject.Find("Cups");
        int random = Random.Range(0, allCups.transform.childCount);

        Destroy(allCups.transform.GetChild(random).gameObject);
    }

    public static void changeColor(Color color)
    {
        diceRenderer.material.color = color;
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
        changeColor(Color.yellow);
        float dirX = Random.Range(0, 200);
        float dirY = Random.Range(0, 200);
        float dirZ = Random.Range(0, 200);
        transform.position = transform.parent.position + new Vector3(0, 2f, -4f);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
