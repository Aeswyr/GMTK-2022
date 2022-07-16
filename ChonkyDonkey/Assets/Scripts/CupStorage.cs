using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStorage : MonoBehaviour
{
    public static Queue<int> cups;
 
    void Awake()
    {
        cups = new Queue<int>(new[] {1, 2, 3, 4});
    }

    void Update()
    {
        
    }
}
