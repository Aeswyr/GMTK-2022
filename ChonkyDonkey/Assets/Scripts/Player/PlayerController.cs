using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float speed;

    void FixedUpdate()
    {
        float dir = InputHandler.Instance.dir;

        Vector2 velocity = rbody.velocity;
        velocity.x = speed * dir;
        rbody.velocity = velocity;

        if (dir != 0) {
            sprite.flipX = dir < 0;
        }
    }
}
