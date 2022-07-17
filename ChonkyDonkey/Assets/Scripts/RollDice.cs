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

    void Update()
    {
        velocity = rb.velocity;

        
        if (Input.GetKeyDown(KeyCode.Space))
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
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
