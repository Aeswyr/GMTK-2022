using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public static Vector3 velocity;

    private static Rigidbody rb;
    private static MeshRenderer diceRenderer;
    private static Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        diceRenderer = GetComponent<MeshRenderer>();
        initialPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        velocity = rb.velocity;
        bool notMoving = (rb.velocity.x == 0f && rb.velocity.y == 0f && rb.velocity.z == 0f);
        if (InputHandler.Instance.interact.down && !FlipCupGameStats.canDrink && notMoving)
        {
            FlipCupGameStats.rolledPlayerDice = true;
            rollDice();
        } 
    }

    public static void changeColor(Color color)
    {
        diceRenderer.material.color = color;
    }

    private void rollDice()
    {
        FlipCupGameStats.spendDice();
        float dirX = Random.Range(50, 300);
        float dirY = Random.Range(50, 300);
        float dirZ = Random.Range(50, 300);
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 100);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
