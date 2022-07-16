using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public void OnInteract() {
        Debug.Log("That's a table alright!");
        
        // DEBUG
        FindObjectOfType<DialogueOverlayUI>().OnGreetDog(1,25);
    }
}
