using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideCheck : MonoBehaviour
{
    public Vector3 velocity;

    void FixedUpdate()
    {
        velocity = RollDice.velocity;
    }

    void OnTriggerStay(Collider col)
    {
        int roll = 0;
        if (velocity.x == 0f && velocity.y == 0f && velocity.z == 0f) {
            switch (col.gameObject.name) {
                case "Side1":
                    Debug.Log("6");
                    roll = 6;
                    break;
                case "Side2":
                    Debug.Log("5");
                    roll = 5;
                    break;
                case "Side3":
                    Debug.Log("4");
                    roll = 4;
                    break;
                case "Side4":
                    Debug.Log("3");
                    roll = 3;
                    break;
                case "Side5":
                    Debug.Log("2");
                    roll = 2;
                    break;
                case "Side6":
                    Debug.Log("1");
                    roll = 1;
                    break;
            }
        }
        if (roll == 6)
        {
            // do some stuff
        }
    }
}
