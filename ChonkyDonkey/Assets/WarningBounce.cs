using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningBounce : MonoBehaviour
{
    public float BounceSpeed;

    // Update is called once per frame
    void Update()
    {
        float s = 1.1f + .5f * Mathf.Sin(Time.time * BounceSpeed);
        transform.localScale = new Vector3(s,s,s);
    }
}
