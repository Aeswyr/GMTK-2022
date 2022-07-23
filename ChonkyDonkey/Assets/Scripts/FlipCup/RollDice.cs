using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public static Vector3 velocity;

    private static Rigidbody rb;
    private static MeshRenderer diceRenderer;
    private static Vector3 initialPosition;

    private float prevRollTime;
    private const float minimumRollCooldown = 0.5f;

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
        bool rerollCondition = Time.time - prevRollTime > minimumRollCooldown || notMoving;
        if (InputHandler.Instance.interact.pressed && !FlipCupGameStats.canDrink && rerollCondition)
        {
            FlipCupGameStats.rolledPlayerDice = true;
            rollDice();
        }

        if (InputHandler.Instance.menu.pressed)
        {
            ModeManager.Instance.ChangeMode(GameMode.Bar);
        }
    }

    private void rollDice()
    {
        AffinityBar.rolledDice = true;
        FlipCupGameStats.spendDice();
        float dirX = Random.Range(100, 300);
        float dirY = Random.Range(50, 300);
        float dirZ = Random.Range(100, 300);
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 300);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
