using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private InteractController interaction;
    [SerializeField] private float speed;

    void FixedUpdate()
    {
        if (ModeManager.Instance.Mode != GameMode.Bar) return;

        // Movement
        float dir = InputHandler.Instance.dir;

        bool moving = dir != 0;

        Vector2 velocity = rbody.velocity;
        velocity.x = speed * dir;
        rbody.velocity = velocity;

        if (dir != 0) {
            sprite.flipX = dir < 0;
        }

        // Interaction

        if (InputHandler.Instance.interact.pressed) {
            
            // hack
            if (FlipCupResultOverlay.Instance.IsShowing)
            {
                FlipCupResultOverlay.Instance.OnClosePressed();
                return;
            }
            
            var interactions = interaction.FindValidInteractables();
            if (interactions.Count > 0)
                interactions[0].Run();
        }

        // Update animator
        animator.SetBool("moving", moving);
    }
}
