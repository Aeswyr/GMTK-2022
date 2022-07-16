using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupHandler : MonoBehaviour
{
    public static int thirst = 0;

    private void OnMouseDown()
    {
        Debug.Log(thirst + " Cup clicked");
    }
}
