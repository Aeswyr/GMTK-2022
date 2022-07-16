using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private InteractController interaction;
    [SerializeField] private float speed;

    void FixedUpdate()
    {
        // Movement
        float dir = InputHandler.Instance.dir;

        Vector2 velocity = rbody.velocity;
        velocity.x = speed * dir;
        rbody.velocity = velocity;

        if (dir != 0) {
            sprite.flipX = dir < 0;
        }

        // Interaction

        if (InputHandler.Instance.interact.pressed) {
            var interactions = interaction.FindValidInteractables();
            if (interactions.Count > 0)
                interactions[0].Run();
        }
    }
}
